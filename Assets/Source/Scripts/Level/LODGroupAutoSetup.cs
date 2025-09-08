using UnityEditor;
using UnityEngine;

public class LODGroupAutoSetup
{
    [MenuItem("Tools/Setup LODGroup for objects")]
    private static void SetupLODGroups()
    {
        foreach (var objects in Selection.gameObjects)
        {
            Setup(objects);
        }
    }

    private static void Setup(GameObject prefab)
    {
        var lodGroup = prefab.GetComponent<LODGroup>();

        if (lodGroup == null)
            lodGroup = prefab.AddComponent<LODGroup>();

        var renderers = prefab.GetComponentsInChildren<Renderer>();
        LOD lod0 = new LOD(0.05f, renderers);
        LOD lod1 = new LOD(0.0f, new Renderer[0]);
        lodGroup.SetLODs(new LOD[] { lod0, lod1});
        lodGroup.RecalculateBounds();
    }
}
