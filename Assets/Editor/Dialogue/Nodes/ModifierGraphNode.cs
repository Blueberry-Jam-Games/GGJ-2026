using UnityEngine;
using Unity.GraphToolkit.Editor;
using System;

[Serializable]
public class SetRangeNode : Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort<RangeType>("Value").Build();
        context.AddInputPort<MathOperation>("Operation").Build();
        context.AddInputPort<float>("Quantity").Build();
        context.AddInputPort<int>("Entry").Build();
        context.AddOutputPort<int>("Continue").Build();
    }
}

[Serializable]
public class SetFlagNode : Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort<string>("Flag").Build();
        context.AddInputPort<bool>("Value").Build();
        context.AddInputPort<int>("Entry").Build();
        context.AddOutputPort<int>("Continue").Build();
    }
}
