using Unity.GraphToolkit.Editor;
using UnityEditor.AssetImporters;
using UnityEngine;

[ScriptedImporter(1, DialogGraph.AssetExtension)]
public class DialogImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        DialogGraph graph = GraphDatabase.LoadGraphForImporter<DialogGraph>(ctx.assetPath);

        if (graph == null)
        {
            
            Debug.LogError($"Failed to import {ctx.assetPath}");
            return;
        }
    }
}
