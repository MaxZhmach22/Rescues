using UnityEngine;


namespace Rescues
{
    [RequireComponent(typeof(AudioSource))]
    public sealed class NodeSoundContainer : MonoBehaviour
    {
        #region Fields

        private AudioSource _audioSource;

        #endregion


        #region UnityMethods

        void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        #endregion


        #region Methods

        public void Initialization(AudioClip audioClip)
        {
            _audioSource.clip = audioClip;
            _audioSource.Play();
        } 

        #endregion
    }
}
