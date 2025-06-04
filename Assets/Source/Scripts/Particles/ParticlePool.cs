using UnityEngine.Pool;
using UnityEngine;
using System.Collections;

public class ParticlePool : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particlePrefab;

    private ObjectPool<ParticleSystem> _pool;

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
    }

    private void OnEnable()
    {
        _pool = new ObjectPool<ParticleSystem>(
             createFunc: () => Instantiate(_particlePrefab, transform.position, Quaternion.identity),
             actionOnGet: (obj) => obj.gameObject.SetActive(true),
             actionOnRelease: (obj) => obj.gameObject.SetActive(false),
             actionOnDestroy: (obj) => Destroy(obj.gameObject)
         );
    }

    public ParticleSystem GetParticle(Vector3 position)
    {
        var particle = _pool.Get();
        particle.transform.position = position;
        StartCoroutine(ReleaseWhenFinished(particle));
        return particle;
    }

    private IEnumerator ReleaseWhenFinished(ParticleSystem particle)
    {
        while (particle.isPlaying)
            yield return null;

        _pool.Release(particle);
    }
}