using UnityEngine;
using Unity.GraphToolkit.Editor;
using System;

[Serializable]
public class EndGraphNode : Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort<int>("Terminate").Build();
    }
}
