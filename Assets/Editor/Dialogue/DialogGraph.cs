using UnityEditor;
using UnityEngine;
using Unity.GraphToolkit.Editor;
using System;

[Serializable]
[Graph(AssetExtension)]
public class DialogGraph : Graph
{
    internal const string AssetExtension  = "dialog";

    [MenuItem("Assets/Create/Blueberry Jam/Dialog")]
    static void CreateDialogAsset()
    {
        GraphDatabase.PromptInProjectBrowserToCreateNewAsset<DialogGraph>();
    }
}
