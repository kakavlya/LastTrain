using UnityEngine;

namespace LastTrain.Enemies
{
    public interface IEnemyBehaviorInitializer
    {
        void Initialize(GameObject enemy, Transform playerTarget, BoxCollider playerCollider);
    }
}