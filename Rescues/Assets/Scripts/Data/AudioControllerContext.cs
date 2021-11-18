using System;
using UnityEngine;


namespace Rescues
{
    [CreateAssetMenu(fileName = nameof(AudioControllerContext), menuName = "Data/Audio/" + nameof(AudioControllerContext), order = 0)]
    public class AudioControllerContext : ScriptableObject
    {
        private AudioController _testAudioController;

        public AudioController GetObjectOfType(Type targetType, string targetName = null)
        {
            if (_testAudioController == null)
            {
                throw new ApplicationException($"{nameof(AudioController)} is not initialized yet!!!");
            }
            var obj = _testAudioController;
            if (targetType.Equals(obj.GetType()))
            {
                throw new ApplicationException($"Please use {nameof(AudioController)}'s interfaces only!");
            }
            if (targetType.IsAssignableFrom(obj.GetType()))
            {
                if (targetName == null)
                {
                    return obj;
                }
            }
            throw new ApplicationException($"{targetType.Name} is not Assignable From {obj.GetType().Name}");
        }

        public void BindAuidoController(AudioController testAudioController)
        {
            if (_testAudioController != null)
            {
                Debug.Log("Already Init!");
                return;
            }
            _testAudioController = testAudioController;
        }
    }
}
