using System.Collections;
using UnityEngine;

namespace Assets.Source.Scripts.Enemies
{
    public class KillEnemyTester : MonoBehaviour
    {
        [SerializeField] private KeyCode _keyCodeToKill;
        private EnemyHealth _enemyHealth;

        private void Start()
        {
            _enemyHealth = GetComponent<EnemyHealth>();
            if (_enemyHealth == null)
            {
                Debug.LogError("EnemyHealth component not found on this GameObject.");
                return;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(_keyCodeToKill))
            {
                _enemyHealth.Die();
            }
        }
    }
}