using UnityEngine;
using Unity.GraphToolkit.Editor;
using System;

[Serializable]
public class BranchNode : Node
{
    const string nChoices = "Choices";

    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort<int>("Entry").Build();

        INodeOption nodeChoices = GetNodeOptionByName(nChoices);
        if (nodeChoices.TryGetValue<int>(out int numberOfBranches))
        {
            for (int i = 0; i < numberOfBranches; i++)
            {
                context.AddInputPort<string>($"Text {i}").Build();
                context.AddInputPort<bool>($"Masked {i}").Build();
                context.AddOutputPort<int>($"Choice {i}").Build();
                context.AddInputPort<string>($"Condition {i}").Build();
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
