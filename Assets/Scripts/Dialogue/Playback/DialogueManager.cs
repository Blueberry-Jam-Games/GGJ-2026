using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueBox;
    public int lineCount = 3;
    public float secondPerLetter = 0.5f;
    public bool dialogueRunning = false;

    public Dialogue tempDialogue;

    public List<string> paragraphLines; // TODO - Private
    public List<string> colourLines;

    private string nextUUID;
    private int buttonPressed = -1;

    public Animator inputAnimator;

    [Header("Left Button")]
    public Button leftbutton;
    public Animator leftAnimator;
    public TextMeshProUGUI leftText;

    [Header("Right Button")]
    public Button rightbutton;
    public Animator rightAnimator;
    public TextMeshProUGUI rightText;

    WaitForSeconds textSpeed;

    private InputAction submitAction;

    private void Awake()
    {
        submitAction = InputSystem.actions.FindAction("Submit");     
    }

    public void Start()
    {
        textSpeed = new WaitForSeconds(secondPerLetter);

        leftbutton.onClick.AddListener(LeftButtonPressed);
        rightbutton.onClick.AddListener(RightButtonPressed);

        // Temp
        //StartDialogue(tempDialogue);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        StartCoroutine(RunInteraction(dialogue));
    }

    IEnumerator RunInteraction(Dialogue dialogue)
    {
        DialogueRuntimeNode activeNode = FindNode(dialogue, dialogue.startGUID);

        while(activeNode.nodeType != DialogueRuntimeNode.NodeType.END)
        {
            if(activeNode.nodeType == DialogueRuntimeNode.NodeType.START)
            {
                activeNode = FindNode(dialogue, activeNode.nodePaths[0].connectedGUID);
            }
            else if (activeNode.nodeType == DialogueRuntimeNode.NodeType.DIALOGUE)
            {
                yield return RunDialogue(activeNode);
                if (nextUUID.Length > 0)
                {
                    activeNode = FindNode(dialogue, nextUUID);
                }

                if (activeNode.nodeType != DialogueRuntimeNode.NodeType.BRANCH)
                {
                    yield return new WaitUntil(() => {return submitAction.WasReleasedThisFrame();});
                }
            }
            else if (activeNode.nodeType == DialogueRuntimeNode.NodeType.BRANCH)
            {
                yield return DoDialogSelect(activeNode.nodePaths);
                activeNode = FindNode(dialogue, activeNode.nodePaths[buttonPressed].connectedGUID);
            }
            else if (activeNode.nodeType == DialogueRuntimeNode.NodeType.SET_FLAG)
            {
                GameplayManager.Instance.SetFlag(activeNode.flagName, activeNode.flagActive);
                activeNode = FindNode(dialogue, activeNode.nodePaths[0].connectedGUID);
            }
            else if (activeNode.nodeType == DialogueRuntimeNode.NodeType.SET_RANGE)
            {
                GameplayManager.Instance.ModifyRange(activeNode.rangeType, activeNode.action, activeNode.value);
                activeNode = FindNode(dialogue, activeNode.nodePaths[0].connectedGUID);
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
    
        if (activeNode.nodePaths.Count == 0)
        {
            nextUUID = "";
        }
        else if (activeNode.nodePaths.Count == 1)
        {
            nextUUID = activeNode.nodePaths[0].connectedGUID;
            // TODO Wait for input -> Need to bother Nate
        }
        else
        {
            Debug.LogError("More than 1 output on a dialog node?");
        }
    }

    private IEnumerator DoDialogSelect(List<NodePath> choices)
    {
        leftText.text = choices[0].response;
        leftbutton.Select();

        if (choices.Count > 1)
        {
            rightbutton.gameObject.SetActive(true);
            rightText.text = choices[1].response;
        }
        else
        {
            rightbutton.gameObject.SetActive(false);
        }

        inputAnimator.Play("OptionsIn");
        yield return BJ.Coroutines.WaitforSeconds(1f);

        //Debug.Log("1 " + choices[0].condition);
        //Debug.Log("2 " + choices[1].condition);
        // TODO conditionals
        if (GameplayManager.Instance.EvaluateCondition(choices[0].condition) == false)
        {
            leftAnimator.Play("ButtonDisable");
            Debug.Log("Left button should be disabled");
            if (choices.Count > 1)
            {
                rightbutton.Select();
            }
            else
            {
                Debug.LogError("Soft Lock Detected");
            }
        }

        if (choices.Count > 1)
        {
            if (GameplayManager.Instance.EvaluateCondition(choices[1].condition) == false)
            {
                rightAnimator.Play("ButtonDisable");
                Debug.Log("Right button should be disabled");
                leftbutton.Select();
            }
        }

        buttonPressed = -1;

        yield return new WaitUntil(() => {return buttonPressed != -1;});

        inputAnimator.Play("OptionsOut");
        yield return BJ.Coroutines.WaitforSeconds(1f);

        nextUUID = choices[buttonPressed].connectedGUID;
    }

    public bool IsDialogueOpen()
    {
        return dialogueRunning;
    }

    private DialogueRuntimeNode FindNode(Dialogue d, string GUID)
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
                string fail = "";
                Debug.LogError("Not enough colours listed on " + paragraph);
                for(int c = 0; c < words.Length; c++)
                {
                    fail += '0';
                }
                colourPerString.Add(fail);
                //break;
                continue;
            }
            colourPerString.Add(colours.Substring(word, words.Length));
            word += words.Length;
        }

        return lines;
    }

    public void LeftButtonPressed()
    {
        if (buttonPressed == -1)
        {
            buttonPressed = 0;
        }
    }

    public void RightButtonPressed()
    {
        if (buttonPressed == -1)
        {
            buttonPressed = 1;
        }
    }
}
