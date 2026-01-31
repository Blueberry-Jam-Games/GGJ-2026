using UnityEngine;
using Unity.GraphToolkit.Editor;

public class DialogueGraphNode : Node
{
    string textBlock = "Text";

    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddOutputPort<float>("Entry").Build();
    }

    protected override void OnDefineOptions(IOptionDefinitionContext context)
    {
        context.AddOption<string>(textBlock)
            .WithDisplayName("Text")
            .WithDefaultValue("I don't know what I'm talking about")
            .Delayed();
    }
}
