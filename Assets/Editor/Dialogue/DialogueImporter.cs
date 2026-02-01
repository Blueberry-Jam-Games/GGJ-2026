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
        string startNodeId = "";

        foreach (INode node in graph.GetNodes())
        {
            nodeIdMap[node] = Guid.NewGuid().ToString();

            if (node is StartGraphNode && startNodeId.Length == 0)
            {
                startNodeId = nodeIdMap[node];
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
            }
            else if (node is EndGraphNode)
            {
                dlgNode.nodeType = DialogueRuntimeNode.NodeType.END;
            }
            else if (node is DialogueGraphNode)
            {
                dlgNode.nodeType = DialogueRuntimeNode.NodeType.DIALOGUE;
                dlgNode.dialogue = GetPortValue<string>(node.GetInputPortByName("Text"));

                foreach (IPort port in node.GetOutputPorts())
                {
                    if (port.name.StartsWith ("Choice "))
                    {
                        string inputChoice = $"Choice Text {getPortIndex(port.name)}";
                        string choiceText = GetPortValue<string>(node.GetInputPortByName(inputChoice));

                        string choiceNode = "";

                        if (port.isConnected)
                        {
                            choiceNode = nodeIdMap[port.firstConnectedPort.GetNode()];
                        }

                        dlgNode.nodePaths.Add(new NodePath(choiceText, choiceNode));
                    }
                }
            }

            dlgNode.GUID = nodeIdMap[node];
            dialogue.nodeTree.Add(dlgNode);
        }

        ctx.AddObjectToAsset("RuntimeAsset", dialogue);
        ctx.SetMainObject(dialogue);
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
