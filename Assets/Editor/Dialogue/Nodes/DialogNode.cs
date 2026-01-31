using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;

[Serializable]
public class DialogNode : Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort<float>("Input").Build();
        context.AddOutputPort<double>("Output").Build();
    }
}
