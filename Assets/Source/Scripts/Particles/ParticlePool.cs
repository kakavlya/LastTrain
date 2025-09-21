using UnityEngine.Pool;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LastTrain.Particles
{
    public class ParticlePool : MonoBehaviour
    {
        public static ParticlePool Instance { get; private set; }

        [SerializeField] private ParticleSystem[] _particlePrefabs;
        [SerializeField] private int _defaultCapacity = 8;
        [SerializeField] private int _maxSize = 64;

        private readonly Dictionary<ParticleSystem, ObjectPool<ParticleSystem>> _pools =
            new Dictionary<ParticleSystem, ObjectPool<ParticleSystem>>();

        public void Init()
        {
            Instance = this;
            InitializePools();
        }

        public ParticleSystem Spawn(ParticleSystem prefab, Vector3 position, Quaternion? rotation = null, Vector3? scale = null)
        {
            if (!_pools.TryGetValue(prefab, out var pool))
            {
                pool = CreatePoolForPrefab(prefab);
            }

            var particle = pool.Get();

            // Готовим трансформ ДО Play
            var transform = particle.transform;
            transform.SetParent(base.transform, worldPositionStays: false);
            transform.position = position;
            if (rotation.HasValue) transform.rotation = rotation.Value;
            if (scale.HasValue) transform.localScale = scale.Value;

            particle.Clear(true);
            particle.Play(true);

            StartCoroutine(ReleaseWhenFinished(particle, prefab));
            return particle;
        }

        private void InitializePools()
        {
            if (_particlePrefabs == null) return;
            foreach (var prefab in _particlePrefabs)
            {
                if (prefab) CreatePoolForPrefab(prefab);
            }
        }

        private ObjectPool<ParticleSystem> CreatePoolForPrefab(ParticleSystem prefab)
        {
            var pool = new ObjectPool<ParticleSystem>(
                createFunc: () => CreateParticle(prefab, parent: transform),
                actionOnGet: particle =>
                {

                    var gameObject = particle.gameObject;
                    if (!gameObject.activeSelf) gameObject.SetActive(true);

                    particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    particle.Clear(true);
                },
                actionOnRelease: particle =>
                {
                    if (!particle) return;
                    particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    particle.gameObject.SetActive(false);
                },
                actionOnDestroy: partlicle =>
                {
                    if (partlicle) Destroy(partlicle.gameObject);
                },
                collectionCheck: false,
                defaultCapacity: _defaultCapacity,
                maxSize: _maxSize
            );

            _pools[prefab] = pool;
            return pool;
        }

        private IEnumerator ReleaseWhenFinished(ParticleSystem particleSystem, ParticleSystem prefabKey)
        {
            while (particleSystem && particleSystem.IsAlive(true))
                yield return null;

            if (particleSystem && _pools.TryGetValue(prefabKey, out var pool))
            {
                pool.Release(particleSystem);
            }
        }

        private ParticleSystem CreateParticle(ParticleSystem prefab, Transform parent)
        {
            if (!prefab)
            {
                Debug.LogError("[ParticlePool] Prefab is null");
                return null;
            }

            var go = Instantiate(prefab.gameObject, parent, false);
            var ps = go.GetComponent<ParticleSystem>();
            if (!ps)
            {
                Debug.LogError($"[ParticlePool] No ParticleSystem on '{prefab.name}' root");
                Destroy(go);
                return null;
            }

            var main = ps.main;
            main.loop = false;
            main.playOnAwake = false;        
            go.SetActive(false);
            return ps;
        }
    }
}
