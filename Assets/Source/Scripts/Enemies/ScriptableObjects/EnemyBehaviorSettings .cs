using UnityEngine;

namespace LastTrain.Enemies
{
    public abstract class EnemyBehaviorSettings : ScriptableObject, IEnemyBehaviorInitializer
    {
        public int Reward;
        public float Health;

        public abstract void Initialize(GameObject enemy, Transform playerTarget, BoxCollider playerCollider);
    }
}