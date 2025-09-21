using UnityEditor;
using UnityEngine;

namespace LastTrain.Level
{
#if UNITY_EDITOR
    [CustomEditor(typeof(ObjectsRandomizer))]
    public class RandomSpawnerEditorInspector : Editor
    {
        private string _generateNearby = "—генерировать ближние";
        private string _generateFar = "—генерировать дальние";
        private string _deleteAll = "Delete";

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            ObjectsRandomizer script = (ObjectsRandomizer)target;

            if (GUILayout.Button(_generateNearby))
            {
                script.SpawnNearObjects();
            }

            if (GUILayout.Button(_generateFar))
            {
                script.SpawnFarObjects();
            }

            if (GUILayout.Button(_deleteAll))
            {
                script.DeleteObjects();
            }
        }
    }
#endif
}
