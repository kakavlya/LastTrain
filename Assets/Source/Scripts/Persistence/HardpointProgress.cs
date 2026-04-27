using System;
using System.Collections.Generic;
using UnityEngine;

namespace LastTrain.Persistence
{
    [Serializable]
    public class HardpointProgress : BaseProgress
    {
        [SerializeField] private int _hardpointIndex;
        [SerializeField] private bool _isUnlocked;
        [SerializeField] private int _level;
        [SerializeField] private string _activeTurretId;
        [SerializeField] private List<TurretProgress> _turretsProgress = new List<TurretProgress>();

        public int HardpointIndex => _hardpointIndex;
        public bool IsUnlocked => _isUnlocked;
        public int Level => _level;
        public string ActiveTurretId => _activeTurretId;
        public List<TurretProgress> TurretsProgress => _turretsProgress;

        public HardpointProgress(int index, bool defaultUnlocked = false)
        {
            _hardpointIndex = index;
            _isUnlocked = defaultUnlocked;
            _level = 0;
            _activeTurretId = string.Empty;
        }

        public void SetUnlocked(bool unlocked) => _isUnlocked = unlocked;
        public void SetActiveTurret(string turretId) => _activeTurretId = turretId;

        public override int GetLevel(StatType stat)
        {
            // Hardpoints only have one generic "Level" which represents their HP bonus
            return _level;
        }

        public override int GetSumLevels() => _level;

        public override void Increment(StatType stat)
        {
            // Any upgrade to a hardpoint just increments its level
            _level++;
        }
    }
}
