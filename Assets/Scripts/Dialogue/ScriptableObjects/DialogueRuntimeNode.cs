using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[Serializable]
public class DialogueRuntimeNode
{
    [Serializable]
    public enum NodeType
    {
        START,
        DIALOGUE,
        BACKGROUND,
        MODIFIER,
        END
    }

    public DialogueRuntimeNode()
    {
        nodePaths = new List<NodePath>();
    }

    public string name; // Node type
    public string GUID; // Unique identifier for node connections
    public NodeType nodeType; // Enum for the type of node represented.

    // Contents used in Dialogue
    public string dialogue;
    public string wordColours;
    public string speakerName;
    public List<NodePath> nodePaths;

    // Modifier Values
}

[Serializable]
public struct NodePath
{
    public string response;
    public string connectedGUID;

    public NodePath(string response, string GUID)
    {
        this.response = response;
        this.connectedGUID = GUID;
    }
}
