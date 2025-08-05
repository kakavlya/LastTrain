using UnityEngine;
namespace Assets.Source.Scripts.Enemies
{
    public abstract class EnemyBehaviorSettings : ScriptableObject, IEnemyBehaviorInitializer
    {
        public abstract void Initialize(GameObject enemy, Transform playerTarget, BoxCollider playerCollider);
    }
}