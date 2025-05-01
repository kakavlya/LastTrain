using System.Collections;
using UnityEngine;

namespace Assets.Source.Scripts.Enemies
{
    public class EnemyMovement : MonoBehaviour
    {

        [SerializeField] private float _speed = 5f;
        private Transform _target;

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        private void Update()
        {
            if (_target == null) return;

            Vector3 direction = (_target.position - transform.position).normalized;
            transform.position += direction * _speed * Time.deltaTime;
            transform.forward = direction; 
        }
    }
}