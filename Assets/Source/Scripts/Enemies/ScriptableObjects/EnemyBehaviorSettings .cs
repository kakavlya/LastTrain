using UnityEngine;

namespace LastTrain.Enemies
{
    public abstract class EnemyBehaviorSettings : ScriptableObject, IEnemyBehaviorInitializer
    {
        [SerializeField] private int _reward;
        [SerializeField] private float _health;

        public int Reward => _reward;
        public float Health => _health;

        public abstract void Initialize(GameObject enemy, Transform playerTarget, BoxCollider playerCollider);
    }
}