using UnityEngine;
using SplineMesh;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class ObjectsRandomizer : MonoBehaviour
{
    [SerializeField] private MeshRenderer _planeMeshRenderer;
    [SerializeField] private Transform _transformParent;

    [Header("Objects Settings")]
    [SerializeField] private GameObject[] _nearObjects;
    [SerializeField] private GameObject[] _farObjects;
    [SerializeField] private int _spawnCount;
    [SerializeField] private float _maxScale;

    [Header("Spline Settings")]
    [SerializeField] private Spline _spline;
    [SerializeField] private float _roadOffsetNear;
    [SerializeField] private float _roadOffsetDistant;

    private Vector3 _planeSize;

#if UNITY_EDITOR
    public void SpawnObjects(GameObject[] spawnObjects, float minDist, float maxDist)
    {
        _planeSize = _planeMeshRenderer.bounds.size;

        for (int i = 0; i < _spawnCount; i++)
        {
            GameObject prefab = spawnObjects[Random.Range(0, spawnObjects.Length)];

            float randomX = Random.Range(-_planeSize.x / 2, _planeSize.x / 2);
            float randomZ = Random.Range(-_planeSize.z / 2, _planeSize.z / 2);

            Vector3 spawnPos = _planeMeshRenderer.transform.position + new Vector3(randomX, 0, randomZ);

            var projection = _spline.GetProjectionSample(spawnPos);
            float distToRoad = Vector3.Distance(spawnPos, projection.location);

            if (distToRoad < minDist || distToRoad > maxDist)
                continue;

            GameObject instance = Instantiate(prefab, _transformParent);
            instance.transform.position = spawnPos;
            instance.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
            float minScale = instance.transform.localScale.x;
            float scale = Random.Range(minScale, _maxScale);
            Vector3 baseScale = instance.transform.localScale;
            instance.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
#endif

    public void SpawnNearObjects()
    {
        SpawnObjects(_nearObjects, _roadOffsetNear, _roadOffsetDistant);
    }

    public void SpawnFarObjects()
    {
        SpawnObjects(_farObjects, _roadOffsetDistant, float.MaxValue);
    }

#if UNITY_EDITOR
    public void DeleteObjects()
    {
        for (int i = _transformParent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(_transformParent.GetChild(i).gameObject);
        }
}
#endif
}


#if UNITY_EDITOR
[CustomEditor(typeof(ObjectsRandomizer))]
public class RandomSpawnerEditorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ObjectsRandomizer script = (ObjectsRandomizer)target;

        if (GUILayout.Button("—генерировать ближние"))
        {
            script.SpawnNearObjects();
        }

        if (GUILayout.Button("—генерировать дальние"))
        {
            script.SpawnFarObjects();
        }

        if (GUILayout.Button("”далить все"))
        {
            script.DeleteObjects();
        }
    }
}
#endif
