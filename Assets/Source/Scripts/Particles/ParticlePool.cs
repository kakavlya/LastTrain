using UnityEngine.Pool;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticlePool : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _particlePrefabs;

    private Dictionary<ParticleSystem, ObjectPool<ParticleSystem>> _pools =
        new Dictionary<ParticleSystem, ObjectPool<ParticleSystem>>();

    public static ParticlePool Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializePools();
    }

    private void InitializePools()
    {
        foreach (var particlePrefab in _particlePrefabs)
        {
            CreatePoolForPrefab(particlePrefab);
        }
    }

    private void CreatePoolForPrefab(ParticleSystem particlePrefab)
    {
        if (!_pools.ContainsKey(particlePrefab))
        {
            _pools[particlePrefab] = new ObjectPool<ParticleSystem>(
                createFunc: () => CreateParticle(particlePrefab, transform),
                actionOnGet: (obj) => obj.gameObject.SetActive(true),
                actionOnRelease: (obj) => obj.gameObject.SetActive(false),
                actionOnDestroy: (obj) => Destroy(obj.gameObject)
            );
        }
    }

    public ParticleSystem GetParticle(ParticleSystem particleSystem, Vector3 position)
    {
        if (!_pools.ContainsKey(particleSystem))
        {
            CreatePoolForPrefab(particleSystem);
        }

        var particle = _pools[particleSystem].Get();
        particle.transform.position = position;
        particle.Play();
        StartCoroutine(ReleaseWhenFinished(particle, particleSystem));
        return particle;
    }

    private IEnumerator ReleaseWhenFinished(ParticleSystem particle, ParticleSystem prefab)
    {
        while (particle.isPlaying)
            yield return null;

        if (particle != null && _pools.ContainsKey(prefab))
        {
            _pools[prefab].Release(particle);
        }
    }

    private ParticleSystem CreateParticle(ParticleSystem particlePrefab, Transform transform)
    {
        var particle = Instantiate(particlePrefab, transform);
        var main = particle.main;
        main.loop = false;
        main.playOnAwake = false;
        return particle;
    }
}