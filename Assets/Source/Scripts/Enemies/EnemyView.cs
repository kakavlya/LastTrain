using System.Collections;
using UnityEngine;

namespace Assets.Source.Scripts.Enemies
{
    public class EnemyView : MonoBehaviour
    {

        [Header("Visual FX")]
        [SerializeField] private GameObject _hitVFX;
        [SerializeField] private GameObject _deathVFX;

        [Header("Audio")]
        [SerializeField] private AudioClip _hitSound;
        [SerializeField] private AudioClip _deathSound;

        [Header("Flash Effect")]
        [SerializeField] private Renderer[] _renderersToFlash;
        [SerializeField] private Color _flashColor = Color.white;
        [SerializeField] private float _flashDuration = 0.1f;

        private Color[] _originalColors;
        private Material[] _materials;

        public EnemyView(GameObject hitVFX)
        {
            this._hitVFX = hitVFX;
        }

        private void Awake()
        {
            if (_renderersToFlash == null || _renderersToFlash.Length == 0)
                _renderersToFlash = GetComponentsInChildren<Renderer>();

            _materials = new Material[_renderersToFlash.Length];
            _originalColors = new Color[_renderersToFlash.Length];

            for (int i = 0; i < _renderersToFlash.Length; i++)
            {
                _materials[i] = _renderersToFlash[i].material;
                _originalColors[i] = _materials[i].color;
            }
        }

        public void PlayHitFX()
        {
            if (_hitVFX)
                Instantiate(_hitVFX, transform.position, Quaternion.identity);

            if (_hitSound)
                AudioSource.PlayClipAtPoint(_hitSound, transform.position);

            StopAllCoroutines();
            StartCoroutine(FlashEffect());
        }

        public void PlayDeathFX()
        {
            if (_deathVFX)
                Instantiate(_deathVFX, transform.position, Quaternion.identity);

            if (_deathSound)
                AudioSource.PlayClipAtPoint(_deathSound, transform.position);
        }

        private IEnumerator FlashEffect()
        {
            // Включить вспышку
            for (int i = 0; i < _materials.Length; i++)
                _materials[i].color = _flashColor;

            yield return new WaitForSeconds(_flashDuration);

            // Вернуть цвет
            for (int i = 0; i < _materials.Length; i++)
                _materials[i].color = _originalColors[i];
        }
    }
}