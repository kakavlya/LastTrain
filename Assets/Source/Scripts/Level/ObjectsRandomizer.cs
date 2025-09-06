using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ObjectsRandomizer : MonoBehaviour
{
    [SerializeField] private GameObject[] _spawnObjects;
    [SerializeField] private int _spawnCount;
    [SerializeField] private Transform _transformParent;

    [Header("Objects Settings")]
    [SerializeField] private float _maxScale;

    private Vector3 _planeSize;

    private void SpawnObjects()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        _planeSize = meshRenderer.bounds.size;

        for (int i = 0; i < _spawnCount; i++)
        {
            GameObject prefab = _spawnObjects[Random.Range(0, _spawnObjects.Length)];

            float randomX = Random.Range(-_planeSize.x / 2, _planeSize.x / 2);
            float randomZ = Random.Range(-_planeSize.z / 2, _planeSize.z / 2);

            Vector3 spawnPos = transform.position + new Vector3(randomX, 0, randomZ);

            GameObject instance = Instantiate(prefab, _transformParent);
            instance.transform.position = spawnPos;
            instance.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
            instance.transform.localScale *= Random.Range(0.8f, _maxScale);
        }
    }
}
