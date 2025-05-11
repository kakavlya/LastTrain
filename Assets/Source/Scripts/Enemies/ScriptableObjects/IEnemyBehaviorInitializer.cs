using UnityEngine;

namespace Assets.Source.Scripts.Enemies
{
    public interface IEnemyBehaviorInitializer
    {
        void Initialize(GameObject enemy, Transform playerTarget);
    }
}