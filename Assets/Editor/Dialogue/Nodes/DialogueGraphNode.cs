using UnityEngine;
using Unity.GraphToolkit.Editor;
using System;

[Serializable]
public class DialogueGraphNode : Node
{
    const string textBlock = "Text";
    const string nChoices = "Choices";

    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort<int>("Entry").Build();
        context.AddInputPort<string>(textBlock).Build();

        INodeOption nodeChoices = GetNodeOptionByName(nChoices);
        if (nodeChoices.TryGetValue<int>(out int numberOfBranches))
        {
            for (int i = 0; i < numberOfBranches; i++)
            {
                context.AddInputPort<string>($"Choice Text {i}").Build();
                context.AddOutputPort<int>($"Choice {i}");          
            }
        }
    }

    protected override void OnDefineOptions(IOptionDefinitionContext context)
    {        
        context.AddOption<int>(nChoices)
            .WithDisplayName("Number of choices")
            .WithDefaultValue(1)
            .Delayed();
    }
}
