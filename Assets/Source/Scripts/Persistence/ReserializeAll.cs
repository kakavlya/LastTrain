#if UNITY_EDITOR
using UnityEditor;
public static class ReserializeAll
{
    [MenuItem("Tools/Migrate/Force Reserialize All Assets")]
    public static void Run() => AssetDatabase.ForceReserializeAssets();
}
#endif