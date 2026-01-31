using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueBox;
    public int lineCount = 3;
    public float secondPerLetter = 0.5f;
    public bool dialogueRunning = false;

    public Dialogue tempDialogue;

    public List<string> paragraphLines; // TODO - Private

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
        List<string> lines = SplitIntoLines(dialogueBox, activeNode.dialogue);
        //TMP
        paragraphLines = lines;
        int startLine = 0;
        int endLine = 0;
        List<string> displayLines = new List<string>();

        foreach(string line in lines)
        {
            int lineLength = LengthWithoutSpaces(line);
            int offset = 0;

            string newLine = "";

            displayLines.Add(newLine);

            for(int i = 0; i < lineLength; i++)
            {
                while(line[i + offset] == ' ')
                {
                    offset += 1;
                    newLine += ' ';
                }

                newLine += line[i + offset];

                displayLines[endLine] = newLine;

                dialogueBox.text = "";

                for(int a = startLine; a <= endLine; a++)
                {
                    dialogueBox.text += displayLines[a];
                    dialogueBox.text += '\n';
                }

                yield return textSpeed;
            }

            endLine ++;

            if(endLine - startLine >= 3)
            {
                startLine ++;
            }

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

    private int LengthWithoutSpaces(String input)
    {
        string withoutSpaces = input.Replace(" ", "");
        return withoutSpaces.Length;
    }

    public static List<string> SplitIntoLines(TMP_Text textMeshPro, string paragraph)
    {
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

        return lines;
    }
}
