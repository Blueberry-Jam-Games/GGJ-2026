using System;
using UnityEditor;
using UnityEngine;
using Unity.GraphToolkit.Editor;

[Graph(AssetExtension)]
[Serializable]
public class DialogueGraph : Graph
{
    internal const string AssetExtension = "dialogue";

    [MenuItem("Assets/Create/Blueberry Jam/Dialog", false)]
    static void CreateAssetFile()
    {
        GraphDatabase.PromptInProjectBrowserToCreateNewAsset<DialogueGraph>();
    }
}
