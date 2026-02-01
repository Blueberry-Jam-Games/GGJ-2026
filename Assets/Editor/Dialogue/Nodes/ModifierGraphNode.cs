using UnityEngine;
using Unity.GraphToolkit.Editor;
using System;

[Serializable]
public class ModifierGraphNode : Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort<int>("Entry").Build();
        context.AddOutputPort<int>("Exit").Build();
    }
}
