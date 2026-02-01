using UnityEngine;
using Unity.GraphToolkit.Editor;
using System;

[Serializable]
public class DialogueGraphNode : Node
{
    const string textBlock = "Text";

    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort<int>("Entry").Build();
        context.AddOutputPort<int>("Continue").Build();
        context.AddInputPort<string>(textBlock).Build();
        context.AddInputPort<string>("WordColours").Build();
        context.AddInputPort<string>("Speaker").Build();
    }
}
