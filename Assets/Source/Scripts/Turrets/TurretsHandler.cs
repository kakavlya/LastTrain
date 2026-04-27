using LastTrain.Data;
using LastTrain.Persistence;
using System.Collections.Generic;
using UnityEngine;
using YG;

namespace LastTrain.Turrets
{
    /// <summary>
    /// Spawns and manages all turret instances for a level session.
    /// Reads hardpoints from the train hierarchy at Init() time and instantiates
    /// TurretController prefabs onto each one.
    ///
    /// Phase 1: combat values come from Inspector SerializeFields.
    /// Phase 2: these will be replaced by TurretUpgradeConfig + TurretProgress lookups,
    ///          matching the pattern WeaponsCreator uses for player weapons.
    /// </summary>
    public class TurretsHandler : MonoBehaviour
    {
        [Header("Hardpoints (assign children of the train prefab)")]
        [SerializeField] private TurretHardpoint[] _hardpoints;

        [Header("Turret prefab")]
        [SerializeField] private TurretController _turretPrefab;

        [Header("Phase 1 — Prototype combat values (replaced by config in Phase 2)")]
        [SerializeField] private float _damage = 30f;
        [SerializeField] private float _range = 40f;
        [SerializeField] private float _fireDelay = 1.5f;
        [SerializeField] private float _projectileSpeed = 60f;

        private readonly List<TurretController> _spawnedTurrets = new List<TurretController>();

        // ── Public API ───────────────────────────────────────────────────────

        /// <summary>
        /// Spawns a turret on every configured hardpoint.
        /// Call from CompositionRoot.Awake(), after pools are initialised.
        /// </summary>
        public void Init()
        {
            // If starting from Main Menu, TransferData will have our configs.
            bool hasTransferData = TransferData.Instance != null && TransferData.Instance.TurretConfigs != null && TransferData.Instance.TurretConfigs.Count > 0;

            int hardpointIndex = 0;

            if (hasTransferData)
            {
                // Each index in the Hardpoints array maps to the same index in the saves progress list
                for (int i = 0; i < _hardpoints.Length; i++)
                {
                    if (i >= YG2.saves.HardpointsProgress.Count)
                        break; // Safety check in case scenes have more hardpoints than the save supports

                    var hardpointProgress = YG2.saves.HardpointsProgress[i];
                    var hardpointObj = _hardpoints[i];

                    if (hardpointObj == null || !hardpointProgress.IsUnlocked || string.IsNullOrEmpty(hardpointProgress.ActiveTurretId))
                        continue;

                    // Find the config for this hardpoint's active turret
                    var config = TransferData.Instance.TurretConfigs.Find(c => c != null && c.TurretId == hardpointProgress.ActiveTurretId);
                    if (config == null)
                    {
                        Debug.LogWarning($"[TurretsHandler] Could not find config for ActiveTurretId {hardpointProgress.ActiveTurretId} on Hardpoint {i}");
                        continue;
                    }

                    // Get the local progress for this specific turret on this specific hardpoint
                    var turretProgress = hardpointProgress.TurretsProgress.Find(t => t.TurretId == config.TurretId);
                    if (turretProgress == null)
                    {
                        Debug.LogWarning($"[TurretsHandler] Hardpoint {i} has ActiveTurretId {config.TurretId} but no progress entry for it!");
                        continue;
                    }

                    SpawnTurretFromConfig(hardpointObj, config, turretProgress);
                }
            }
            else
            {
                // Editor fallback — no configs passed, just spawn the Inspector prefab if one exists.
                // For testing purposes, we bypass the unlock check here.
                if (_turretPrefab == null)
                {
                    Debug.LogWarning("[TurretsHandler] No turret configs and no fallback prefab assigned.");
                    return;
                }

                foreach (TurretHardpoint hardpoint in _hardpoints)
                {
                    if (hardpoint == null)
                        continue;

                    SpawnTurret(hardpoint);
                }
            }
        }

        /// <summary>
        /// Enables turret AI. Call when the level starts (mirrors EnemySpawner.Begin).
        /// </summary>
        public void Begin()
        {
            foreach (TurretController t in _spawnedTurrets)
                t.Begin();
        }

        /// <summary>Freezes all turrets.</summary>
        public void Pause()
        {
            foreach (TurretController t in _spawnedTurrets)
                t.Pause();
        }

        /// <summary>Resumes all turrets after a pause.</summary>
        public void Resume()
        {
            foreach (TurretController t in _spawnedTurrets)
                t.Resume();
        }

        // ── Private ──────────────────────────────────────────────────────────

        private void SpawnTurret(TurretHardpoint hardpoint)
        {
            TurretController instance = Instantiate(
                _turretPrefab,
                hardpoint.AttachPoint.position,
                hardpoint.AttachPoint.rotation,
                hardpoint.AttachPoint);   // child of the hardpoint → moves with the train

            instance.Init(_damage, _range, _fireDelay, _projectileSpeed);
            _spawnedTurrets.Add(instance);
        }

        private void SpawnTurretFromConfig(TurretHardpoint hardpoint, TurretUpgradeConfig config, TurretProgress progress)
        {
            if (config.TurretPrefab == null)
            {
                Debug.LogError($"[TurretsHandler] Config {config.name} has no TurretPrefab assigned!");
                return;
            }

            GameObject prefabInstance = Instantiate(
                config.TurretPrefab,
                hardpoint.AttachPoint.position,
                hardpoint.AttachPoint.rotation,
                hardpoint.AttachPoint);

            if (prefabInstance.TryGetComponent<TurretController>(out var turretController))
            {
                // Phase 3 stats mapping will happen here. For now, use Phase 1 defaults or config base stats if implemented.
                // Using Phase 1 Inspector defaults as base, but this sets up the path for Phase 3.
                float dmg = config.TryFindStat(StatType.Damage) ? config.GetStat(StatType.Damage, progress.DamageLevel) : _damage;
                float fireDelay = config.TryFindStat(StatType.FireRate) ? config.GetStat(StatType.FireRate, progress.FireRateLevel) : _fireDelay;
                
                turretController.Init(dmg, _range, fireDelay, _projectileSpeed);
                _spawnedTurrets.Add(turretController);
            }
            else
            {
                Debug.LogError($"[TurretsHandler] Prefab {config.TurretPrefab.name} does not have a TurretController!");
            }
        }
    }
}
