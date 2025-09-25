using System.Collections.Generic;
using UnityEngine;

namespace LastTrain.UI.Gameplay
{
    public class PointerHandler : MonoBehaviour
    {
        private const float _visiblyDistance = 500f;
        private const float _screenCenterDivider = 2f;
        private const float _angleCorrection = 90f;
        private const float _screenBoundary = 0f;

        public static PointerHandler Instance;

        [SerializeField] private EnemyPointerIcon _iconPrefab;
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Camera _camera;

        private Dictionary<GameObject, EnemyPointerIcon> _pointerDictonary =
            new Dictionary<GameObject, EnemyPointerIcon>();

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }

        public void AddToDictonary(GameObject enemy)
        {
            EnemyPointerIcon newPointer = Instantiate(_iconPrefab, transform);
            _pointerDictonary.Add(enemy, newPointer);
        }

        public void RemoveFromList(GameObject enemy)
        {
            if (_pointerDictonary.ContainsKey(enemy))
            {
                Destroy(_pointerDictonary[enemy].gameObject);
                _pointerDictonary.Remove(enemy);
            }
        }

        private void Update()
        {
            foreach (var pointer in _pointerDictonary)
            {
                GameObject enemy = pointer.Key;
                Vector3 distance = enemy.transform.position - _playerTransform.position;

                if (distance.magnitude > _visiblyDistance)
                    continue;

                EnemyPointerIcon pointerIcon = pointer.Value;
                float percentDist = distance.magnitude / _visiblyDistance;
                pointerIcon.ChangeAlpha(percentDist);

                Vector3 screenPos = _camera.WorldToScreenPoint(enemy.transform.position);

                if (screenPos.z < 0)
                {
                    screenPos.x = Screen.width - screenPos.x;
                    screenPos.y = Screen.height - screenPos.y;
                    float clampedX = Mathf.Clamp(screenPos.x, _screenBoundary, Screen.width);
                    float clampedY = Mathf.Clamp(screenPos.y, _screenBoundary, Screen.height);
                    Vector3 clampedPos = new Vector3(clampedX, clampedY, 0);
                    Vector3 fromCenter = (clampedPos - new Vector3(
                        Screen.width / _screenCenterDivider, Screen.height / _screenCenterDivider)).normalized;
                    float angle = Mathf.Atan2(fromCenter.y, fromCenter.x) * Mathf.Rad2Deg;
                    pointerIcon.SetIconPosition(clampedPos, Quaternion.Euler(0, 0, angle - _angleCorrection));
                    pointerIcon.Show();
                    continue;
                }

                bool offScreen = screenPos.x < _screenBoundary || screenPos.x > Screen.width ||
                                 screenPos.y < _screenBoundary || screenPos.y > Screen.height;

                if (offScreen)
                {
                    float clampedX = Mathf.Clamp(screenPos.x, _screenBoundary, Screen.width);
                    float clampedY = Mathf.Clamp(screenPos.y, _screenBoundary, Screen.height);
                    Vector3 clampedPos = new Vector3(clampedX, clampedY, screenPos.z);
                    Vector3 fromCenter = (clampedPos - new Vector3(
                        Screen.width / _screenCenterDivider, Screen.height / _screenCenterDivider)).normalized;
                    float angle = Mathf.Atan2(fromCenter.y, fromCenter.x) * Mathf.Rad2Deg;
                    pointerIcon.SetIconPosition(clampedPos, Quaternion.Euler(0, 0, angle - _angleCorrection));
                    pointerIcon.Show();
                }
                else
                {
                    pointerIcon.Hide();
                }
            }
        }
    }
}