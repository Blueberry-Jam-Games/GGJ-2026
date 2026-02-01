using Unity.GraphToolkit.Editor;
using UnityEditor.AssetImporters;
using UnityEngine;
using System.Collections.Generic;
using System;

[ScriptedImporter(1, DialogueGraph.AssetExtension)]
public class DialogueImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        DialogueGraph graph = GraphDatabase.LoadGraphForImporter<DialogueGraph>(ctx.assetPath);

        if (graph == null)
        {
            Debug.LogError($"Dialog {ctx.assetPath} failed to load");
            return;
        }

        Dialogue dialogue = ScriptableObject.CreateInstance<Dialogue>();
        dialogue.nodeTree = new List<DialogueRuntimeNode>();

        Dictionary<INode, string> nodeIdMap = new Dictionary<INode, string>();

        foreach (INode node in graph.GetNodes())
        {
            nodeIdMap[node] = Guid.NewGuid().ToString();
        }

        foreach (INode node in graph.GetNodes())
        {
            Debug.Log("For each node");
            if (node is CombineNode cn)
            {
                IPort nextNodePort = node.GetOutputPortByName("Continue")?.firstConnectedPort;
                if (nextNodePort != null)
                {
                    Debug.Log($"Mapping node {nodeIdMap[cn]} to {nodeIdMap[nextNodePort.GetNode()]}");
                    nodeIdMap[cn] = nodeIdMap[nextNodePort.GetNode()];
                }
            }
        }

        foreach (INode node in graph.GetNodes())
        {
            DialogueRuntimeNode dlgNode = new DialogueRuntimeNode();

            if (node is StartGraphNode)
            {
                dlgNode.nodeType = DialogueRuntimeNode.NodeType.START;
                IPort nextNodePort = node.GetOutputPortByName("Entry")?.firstConnectedPort;
                if (nextNodePort != null)
                {
                    dlgNode.nodePaths.Add(new NodePath("", nodeIdMap[nextNodePort.GetNode()]));
                }
                dialogue.startGUID = nodeIdMap[node];
            }
            else if (node is EndGraphNode)
            {
                dlgNode.nodeType = DialogueRuntimeNode.NodeType.END;
            }
            else if (node is DialogueGraphNode)
            {
                MakeDialogNode(dlgNode, nodeIdMap, node);
            }
            else if (node is BranchNode)
            {
                MakeBranchNode(dlgNode, nodeIdMap, node);
            }
            else if (node is SetRangeNode)
            {
                MakeSetRangeNode(dlgNode, nodeIdMap, node);
            }
            else if (node is SetFlagNode)
            {
                MakeSetFlagNode(dlgNode, nodeIdMap, node);
            }
            else if (node is CombineNode)
            {
                // skip
            }
            else
            {
                Debug.LogWarning($"Unidentified node of type {node.GetType()}");
            }

            dlgNode.GUID = nodeIdMap[node];
            dialogue.nodeTree.Add(dlgNode);
        }

        ctx.AddObjectToAsset("RuntimeAsset", dialogue);
        ctx.SetMainObject(dialogue);
    }

    public void MakeDialogNode(DialogueRuntimeNode dlgNode, Dictionary<INode, string> nodeIdMap, INode node)
    {
        dlgNode.nodeType = DialogueRuntimeNode.NodeType.DIALOGUE;
        dlgNode.dialogue = GetPortValue<string>(node.GetInputPortByName("Text"));
        dlgNode.wordColours = GetPortValue<string>(node.GetInputPortByName("WordColours"));
        dlgNode.speakerName = GetPortValue<string>(node.GetInputPortByName("Speaker"));

        IPort nextNodePort = node.GetOutputPortByName("Continue")?.firstConnectedPort;
        if (nextNodePort != null)
        {
            dlgNode.nodePaths.Add(new NodePath("", nodeIdMap[nextNodePort.GetNode()]));
        }
    }

    public void MakeBranchNode(DialogueRuntimeNode dlgNode, Dictionary<INode, string> nodeIdMap, INode node)
    {
        dlgNode.nodeType = DialogueRuntimeNode.NodeType.BRANCH;

        foreach (IPort port in node.GetOutputPorts())
        {
            if (port.name.StartsWith ("Choice "))
            {
                int portIndex = getPortIndex(port.name);

                string inputChoice = $"Text {portIndex}";
                string choiceText = GetPortValue<string>(node.GetInputPortByName(inputChoice));

                string inputMasked = $"Masked {portIndex}";
                bool masked = GetPortValue<bool>(node.GetInputPortByName(inputMasked));

                string choiceNode = "";

                if (port.isConnected)
                {
                    choiceNode = nodeIdMap[port.firstConnectedPort.GetNode()];
                }

                // Unwind conditions
                string condition = $"Condition {portIndex}";
                string condValue = GetPortValue<string>(node.GetInputPortByName(condition));


                dlgNode.nodePaths.Add(new NodePath(choiceText, choiceNode, masked, condValue));
            }
        }
    }

    public void MakeSetRangeNode(DialogueRuntimeNode dlgNode, Dictionary<INode, string> nodeIdMap, INode node)
    {
        dlgNode.nodeType = DialogueRuntimeNode.NodeType.SET_RANGE;

        dlgNode.rangeType = GetPortValue<RangeType>(node.GetInputPortByName("Value"));
        dlgNode.action = GetPortValue<MathOperation>(node.GetInputPortByName("Operation"));
        dlgNode.value = GetPortValue<float>(node.GetInputPortByName("Quantity"));

        IPort nextNodePort = node.GetOutputPortByName("Continue")?.firstConnectedPort;
        if (nextNodePort != null)
        {
            dlgNode.nodePaths.Add(new NodePath("", nodeIdMap[nextNodePort.GetNode()]));
        }
    }

    public void MakeSetFlagNode(DialogueRuntimeNode dlgNode, Dictionary<INode, string> nodeIdMap, INode node)
    {
        dlgNode.nodeType = DialogueRuntimeNode.NodeType.SET_FLAG;

        dlgNode.flagName = GetPortValue<string>(node.GetInputPortByName("Flag"));
        dlgNode.flagActive = GetPortValue<bool>(node.GetInputPortByName("Value"));

        IPort nextNodePort = node.GetOutputPortByName("Continue")?.firstConnectedPort;
        if (nextNodePort != null)
        {
            dlgNode.nodePaths.Add(new NodePath("", nodeIdMap[nextNodePort.GetNode()]));
        }
    }

    private int getPortIndex(string port)
    {
        string[] splits = port.Split(' ');
        return int.Parse(splits[1]);    
    }

    private T GetPortValue<T>(IPort port)
    {
        if (port == null) return default;

        if (port.isConnected)
        {
            if (port.firstConnectedPort.GetNode() is IVariableNode variableNode)
            {
                variableNode.variable.TryGetDefaultValue(out T value);
                return value;
            }
            else if (port.firstConnectedPort.GetNode() is IConstantNode constantNode)
            {
                constantNode.TryGetValue(out T value);
                return value;
            }
        }

        port.TryGetValue(out T fallback);
        return fallback;
    }
}
