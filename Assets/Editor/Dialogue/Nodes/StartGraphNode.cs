using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;

[Serializable]
public class StartGraphNode : Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddOutputPort<int>("Entry").Build();
    }
}
