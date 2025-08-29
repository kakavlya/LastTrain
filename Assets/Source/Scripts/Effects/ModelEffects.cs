using System.Collections;
using UnityEngine;

namespace LastTrain.Effects
{
    public class ModelEffects : MonoBehaviour
    {
        [Header("Visual FX")]
        [SerializeField] private GameObject _hitVFX;
        [SerializeField] private GameObject[] _deathVFXOptions;
        [SerializeField] private float _deathVFXlife = 2f;

        [Header("Audio")]
        [SerializeField] private AudioClip _hitSound;
        [SerializeField] private AudioClip _deathSound;

        [Header("Flash Effect")]
        [SerializeField] private Renderer[] _renderersToFlash;
        [SerializeField] private Color _flashColor = Color.white;
        [SerializeField] private float _flashDuration = 0.1f;

        private Color[] _originalColors;
        private Material[] _materials;

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
            var selectedVFX = GetRandomVFX(_deathVFXOptions);
            if (selectedVFX != null)
            {
                var fx = Instantiate(selectedVFX, transform.position, Quaternion.identity);
                fx.transform.SetParent(null);
                Destroy(fx, _deathVFXlife);
            }

            if (_deathSound)
                AudioSource.PlayClipAtPoint(_deathSound, transform.position);
        }

        private IEnumerator FlashEffect()
        {
            for (int i = 0; i < _materials.Length; i++)
                _materials[i].color = _flashColor;

            yield return new WaitForSeconds(_flashDuration);

            for (int i = 0; i < _materials.Length; i++)
                _materials[i].color = _originalColors[i];
        }

        private GameObject GetRandomVFX(GameObject[] options)
        {
            if (options == null || options.Length == 0)
                return null;

            int index = Random.Range(0, options.Length);
            return options[index];
        }
    }
}