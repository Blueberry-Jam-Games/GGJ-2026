using UnityEngine;
using Unity.GraphToolkit.Editor;

public class EndGraphNode : Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort<float>("Terminate").Build();
    }
}
