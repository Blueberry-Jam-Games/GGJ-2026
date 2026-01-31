using Unity.GraphToolkit.Editor;
using UnityEditor.AssetImporters;
using UnityEngine;

[ScriptedImporter(1, DialogueGraph.AssetExtension)]
public class DialogueImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        DialogueGraph graph = GraphDatabase.LoadGraphForImporter<DialogueGraph>(ctx.assetPath);

        if (graph == null)
        {
            Debug.LogError($"Dialog {ctx.assetPath} failed to load");
            return;
        }

        Dialogue dialogue = ScriptableObject.CreateInstance<Dialogue>();

        ctx.AddObjectToAsset("RuntimeAsset", dialogue);
        ctx.SetMainObject(dialogue);
    }
}
