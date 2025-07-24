using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PickableAmmunitionPool : MonoBehaviour
{
    [SerializeField] private PickableAmmunition[] _pickableAmmunitionPrefabs;

    private Dictionary<PickableAmmunition, ObjectPool<PickableAmmunition>> _pools =
        new Dictionary<PickableAmmunition, ObjectPool<PickableAmmunition>>();

    public static PickableAmmunitionPool Instance { get; private set; }

    public void Init()
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
        foreach (var ammoPrefab in _pickableAmmunitionPrefabs)
        {
            CreatePoolForPrefab(ammoPrefab);
        }
    }

    private void CreatePoolForPrefab(PickableAmmunition pickableAmmunitionPrefab)
    {
        if (!_pools.ContainsKey(pickableAmmunitionPrefab))
        {
            _pools[pickableAmmunitionPrefab] = new ObjectPool<PickableAmmunition>(
                createFunc: () => Instantiate(pickableAmmunitionPrefab, transform),
                actionOnGet: (obj) => obj.gameObject.SetActive(true),
                actionOnRelease: (obj) => obj.gameObject.SetActive(false),
                actionOnDestroy: (obj) => Destroy(obj.gameObject)
            );
        }
    }

    public PickableAmmunition Spawn(PickableAmmunition pickableAmmunition, Vector3 position)
    {
        if (!_pools.ContainsKey(pickableAmmunition))
        {
            CreatePoolForPrefab(pickableAmmunition);
        }

        var currentPickableAmmo = _pools[pickableAmmunition].Get();
        currentPickableAmmo.transform.position = position;
        currentPickableAmmo.SetPrefabKey(pickableAmmunition);
        return currentPickableAmmo;
    }

    public void RealeseAmmunition(PickableAmmunition pickableAmmunition, PickableAmmunition ammunitionKey)
    {
        if (_pools.ContainsKey(ammunitionKey))
        {
            _pools[ammunitionKey].Release(pickableAmmunition);
        }
        else
        {
            Destroy(pickableAmmunition.gameObject);
        }
    }
}
