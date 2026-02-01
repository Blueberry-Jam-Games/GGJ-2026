using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueBox;
    public int lineCount = 3;
    public float secondPerLetter = 0.5f;
    public bool dialogueRunning = false;

    public Dialogue tempDialogue;

    public List<string> paragraphLines; // TODO - Private
    public List<string> colourLines;

    public string nextUUID;

    WaitForSeconds textSpeed;

    public void Start()
    {
        textSpeed = new WaitForSeconds(secondPerLetter);
        StartDialogue(tempDialogue);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        DialogueRuntimeNode startNode = null;
        foreach(DialogueRuntimeNode node in dialogue.nodeTree)
        {
            if(node.nodeType == DialogueRuntimeNode.NodeType.DIALOGUE)
            {
                startNode = node;
                break;
            }
        }

        if(startNode == null)
        {
            Debug.LogError("No Start Node");
            return;
        }

        dialogueRunning = true;
        StartCoroutine(RunInteraction(dialogue, startNode));
    }

    IEnumerator RunInteraction(Dialogue dialogue, DialogueRuntimeNode startNode)
    {
        DialogueRuntimeNode activeNode = startNode;

        while(activeNode.nodeType != DialogueRuntimeNode.NodeType.END)
        {
            if(activeNode.nodeType == DialogueRuntimeNode.NodeType.START)
            {
                activeNode = findNode(dialogue, activeNode.GUID);
            }
            else if (activeNode.nodeType == DialogueRuntimeNode.NodeType.DIALOGUE)
            {
                yield return RunDialogue(activeNode);
            }
            else
            {
                break;
            }
        }

        dialogueRunning = false;
    }

    IEnumerator RunDialogue(DialogueRuntimeNode activeNode)
    {
        List<string> colourPerString;
        List<string> lines = SplitIntoLines(dialogueBox, activeNode.dialogue, activeNode.wordColours, out colourPerString);
        //TMP
        paragraphLines = lines;
        colourLines = colourPerString;
        int startLine = 0;
        int endLine = 0;
        List<string> displayLines = new List<string>();
        List<string> displayLinesColour = new List<string>();

        int lineNumber = 0;
        foreach(string line in lines)
        {
            int lineLength = LengthWithoutSpaces(line);
            int offset = 0;

            string newLine = "";
            string colourNewLine = "<color=\"" + GetEmotion(colourPerString[lineNumber][0]) + "\">";

            displayLines.Add(newLine);
            displayLinesColour.Add(colourNewLine);

            int currentWord = 0;
            for(int i = 0; i < lineLength; i++)
            {
                bool whitespace = false;
                while(line[i + offset] == ' ')
                {
                    whitespace = true;
                    offset += 1;
                    newLine += ' ';
                    colourNewLine += ' ';
                }
                if(whitespace)
                {
                    currentWord ++;
                    colourNewLine += "</color><color=\"" + GetEmotion(colourPerString[lineNumber][currentWord]) + "\">";
                }

                newLine += line[i + offset];
                colourNewLine += line[i + offset];

                displayLines[endLine] = newLine;
                displayLinesColour[endLine] = colourNewLine;

                dialogueBox.text = "";

                for(int a = startLine; a <= endLine; a++)
                {
                    dialogueBox.text += displayLinesColour[a];
                    dialogueBox.text += '\n';
                }

                yield return textSpeed;
            }

            endLine ++;

            if(endLine - startLine >= 3)
            {
                startLine ++;
            }
            
            lineNumber ++;
        }
    }

    public bool isDialogueOpen()
    {
        return dialogueRunning;
    }

    private DialogueRuntimeNode findNode(Dialogue d, string GUID)
    {
        foreach(DialogueRuntimeNode node in d.nodeTree)
        {
            if(node.GUID == GUID)
            {
                return node;
            }
        }

        DialogueRuntimeNode end = new DialogueRuntimeNode();
        end.nodeType = DialogueRuntimeNode.NodeType.END;
        return end;
    }

    public string GetEmotion(int colour)
    {
        switch (colour)
        {
            case '0':
                return "black";
            case '1':
                return "blue";
            case '2':
                return "yellow";
            case '3':
                return "red";
            case '4':
                return "orange";
            case '5':
                return "blue";
            case '6':
                return "blue";
            case '7':
                return "blue";
            case '8':
                return "blue";
            case '9':
                return "blue";
            default:
                return "black";
        }
    }

    private int LengthWithoutSpaces(String input)
    {
        string withoutSpaces = input.Replace(" ", "");
        return withoutSpaces.Length;
    }

    public static List<string> SplitIntoLines(TMP_Text textMeshPro, string paragraph, string colours, out List<string> colourPerString)
    {
        colourPerString = new List<string>();
        List<string> lines = new List<string>();

        if (string.IsNullOrEmpty(paragraph))
            return lines;

        // Assign the text to TMP and force an update so textInfo is valid
        string originalText = textMeshPro.text;
        textMeshPro.text = paragraph;
        textMeshPro.ForceMeshUpdate();

        TMP_TextInfo textInfo = textMeshPro.textInfo;

        for (int i = 0; i < textInfo.lineCount; i++)
        {
            int firstChar = textInfo.lineInfo[i].firstCharacterIndex;
            int lastChar = textInfo.lineInfo[i].lastCharacterIndex;

            // Substring of the paragraph that belongs to this line
            string lineText = paragraph.Substring(firstChar, lastChar - firstChar + 1);

            lines.Add(lineText);
        }

        // Restore original text (optional)
        textMeshPro.text = originalText;
        textMeshPro.ForceMeshUpdate();

        int word = 0;
        foreach(string line in lines)
        {
            char[] delimiters = new char[] { ' ', '\t', '\n', '\r' };
            string[] words = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            if(words.Length + word >= colours.Length || word >= colours.Length)
            {
                Debug.LogError("Not enough colours listed on " + paragraph);
                break;
            }
            colourPerString.Add(colours.Substring(word, words.Length));
            word += words.Length;
        }

        return lines;
    }
}
