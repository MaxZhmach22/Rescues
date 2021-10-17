using UnityEngine;

namespace DefaultNamespace
{
    internal sealed class DestroyClip : MonoBehaviour
    {
        private AudioSource _audioSource;
    
        void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void Initialization(AudioClip audioClip)
        {
            _audioSource.clip = audioClip;
            _audioSource.Play();
            Destroy(gameObject, _audioSource.clip.length + 1.0f);
        }
    }
}
