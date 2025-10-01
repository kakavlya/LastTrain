using SplineMesh;
using System.Linq;
using UnityEngine;

namespace LastTrain.Level
{
    public class ObjectsRandomizer : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _planeMeshRenderer;
        [SerializeField] private Transform _transformParent;

        [Header("Objects Settings")]
        [SerializeField] private GameObject[] _nearObjects;
        [SerializeField] private GameObject[] _farObjects;
        [SerializeField] private int _spawnCount;
        [SerializeField] private float _maxScale;
        [Range(0f, 0.3f)]
        [SerializeField] private float _colorVariation;

        [Header("Spline Settings")]
        [SerializeField] private Spline _spline;
        [SerializeField] private float _roadOffsetNear;
        [SerializeField] private float _roadOffsetDistant;

        private Vector3 _planeSize;
        private float _planeSizeDivider = 2;
        private float _maxRotationAngle = 360f;

#if UNITY_EDITOR
        public void SpawnLevelObjects(GameObject[] spawnObjects, float minDist, float maxDist)
        {
            spawnObjects = spawnObjects.Where(obj => obj != null).ToArray();
            _planeSize = _planeMeshRenderer.bounds.size;

            for (int i = 0; i < _spawnCount; i++)
            {
                GameObject prefab = spawnObjects[Random.Range(0, spawnObjects.Length)];
                float randomX = Random.Range(-_planeSize.x / _planeSizeDivider, _planeSize.x / _planeSizeDivider);
                float randomZ = Random.Range(-_planeSize.z / _planeSizeDivider, _planeSize.z / _planeSizeDivider);
                Vector3 spawnPos = _planeMeshRenderer.transform.position + new Vector3(randomX, 0, randomZ);
                var projection = _spline.GetProjectionSample(spawnPos);
                float distToRoad = Vector3.Distance(spawnPos, projection.location);

                if (distToRoad < minDist || distToRoad > maxDist)
                    continue;

                GameObject instance = Instantiate(prefab, _transformParent);
                instance.transform.position = spawnPos;
                instance.transform.rotation = Quaternion.Euler(0, Random.Range(0f, _maxRotationAngle), 0);
                float minScale = instance.transform.localScale.x;
                float scale = Random.Range(minScale, _maxScale);
                Vector3 baseScale = instance.transform.localScale;
                instance.transform.localScale = new Vector3(scale, scale, scale);
                RandomizeColorSimple(instance);
            }
        }
#endif

#if UNITY_EDITOR
        public void SpawnNearObjects()
        {
            SpawnLevelObjects(_nearObjects, _roadOffsetNear, _roadOffsetDistant);
        }

        public void SpawnFarObjects()
        {
            SpawnLevelObjects(_farObjects, _roadOffsetDistant, float.MaxValue);
        }

        public void DeleteObjects()
        {
            for (int i = _transformParent.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(_transformParent.GetChild(i).gameObject);
            }
        }
#endif

        private void RandomizeColorSimple(GameObject inctance)
        {
            MeshRenderer renderer = inctance.GetComponent<MeshRenderer>();

            if (renderer != null)
            {
                Material material = new Material(renderer.sharedMaterial);

                Color variation = new Color(
                    Random.Range(1f - _colorVariation, 1f + _colorVariation),
                    Random.Range(1f - _colorVariation, 1f + _colorVariation),
                    Random.Range(1f - _colorVariation, 1f + _colorVariation));

                material.color *= variation;
                renderer.sharedMaterial = material;
            }
        }
    }
}
