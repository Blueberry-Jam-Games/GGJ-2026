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
        BRANCH,
        SET_FLAG,
        SET_RANGE,
        END
    }

    public DialogueRuntimeNode()
    {
        nodePaths = new List<NodePath>();
    }
    [Header("COMMON")]
    public string GUID; // Unique identifier for node connections
    public NodeType nodeType; // Enum for the type of node represented.
    public List<NodePath> nodePaths;

    [Header("Dialog")]
    // Contents used in Dialogue
    public string dialogue;
    public string wordColours;
    public string speakerName;

    [Header("Set Range")]
    public RangeType rangeType;
    public MathOperation action;
    public float value;

    [Header("Set Flag")]
    public string flagName;
    public bool flagActive;
    // Modifier Values
}

[Serializable]
public struct NodePath
{
    public string response;
    public string connectedGUID;
    public bool masked;
    public string condition;

    public NodePath(string response, string GUID, bool mask = false, string cond = "")
    {
        this.response = response;
        this.connectedGUID = GUID;
        this.masked = mask;
        condition = cond;
    }
}

public enum RangeType
{
    STIMULATION,
    ENERGY
}

public enum MathOperation
{
    SET, ADD, SUB, MUL
}
