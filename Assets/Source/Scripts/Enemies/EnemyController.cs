using UnityEngine;

namespace LastTrain.Enemies
{
    public abstract class EnemyController : MonoBehaviour
    {
        protected EnemyHealth Health { get; private set; }
        protected bool IsAlive { get; private set; }

        protected virtual void Awake()
        {
            Health = GetComponent<EnemyHealth>();
            if (Health != null)
                Health.OnDeath.AddListener(HandleDeath_Internal);
        }

        protected virtual void OnDestroy()
        {
            if (Health != null)
                Health.OnDeath.RemoveListener(HandleDeath_Internal);
        }

        protected virtual void OnEnable()
        {
            IsAlive = true;
            ResetStateForSpawn();
        }


        protected virtual void OnDisable()
        {
            OnDespawn();
        }

        private void HandleDeath_Internal()
        {
            if (!IsAlive) return;
            IsAlive = false;
            OnDeath();         
        }

        
        protected abstract void ResetStateForSpawn(); 
        protected virtual void OnSpawn() { }          
        protected virtual void OnDespawn() { }        
        protected virtual void OnDeath() { }
    }
}
