using UnityEngine;

namespace LastTrain.Audio
{
    public class AudioBackground : MonoBehaviour
    {
        [SerializeField] private AudioClip[] _audioClips;

        private void Start()
        {
            PlayRandomClip();
        }

        private void PlayRandomClip()
        {
            if (_audioClips.Length > 0)
            {
                var numberClip = Random.Range(0, _audioClips.Length);
                AudioManager.Instance.PlayBackgroundMusic(_audioClips[numberClip]);
            }
        }
    }
}
