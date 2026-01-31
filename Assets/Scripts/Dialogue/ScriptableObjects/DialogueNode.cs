using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[Serializable]
public class DialogueNode
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

    public string name; // Node type
    public string GUID; // Unique identifier for node connections
    public NodeType nodeType; // Enum for the type of node represented.

    // Contents used in Dialogue
    public string dialogue;
    public List<NodePath> nodePaths;

    // Modifier Values
}

[Serializable]
public struct NodePath
{
    public string response;
    public string connectedGUID;
}