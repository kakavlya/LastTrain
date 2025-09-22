using LastTrain.Particles;
using UnityEngine;

namespace LastTrain.Effects
{
    public class ModelEffects : MonoBehaviour
    {
        [Header("Visual FX")]
        [SerializeField] private ParticleSystem _hitVFX;
        [SerializeField] private GameObject[] _deathVFXOptions;
        [SerializeField] private float _deathVFXlife = 2f;

        [Header("Audio")]
        [SerializeField] private AudioClip _hitSound;
        [SerializeField] private AudioClip _deathSound;

        public void PlayHitFX()
        {
            if (_hitVFX)
                ParticlePool.Instance.Spawn(_hitVFX, transform.position);

            if (_hitSound)
                AudioSource.PlayClipAtPoint(_hitSound, transform.position);

            StopAllCoroutines();
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

        private GameObject GetRandomVFX(GameObject[] options)
        {
            if (options == null || options.Length == 0)
                return null;

            int index = Random.Range(0, options.Length);
            return options[index];
        }
    }
}