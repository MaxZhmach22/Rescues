using UnityEngine;


namespace Rescues
{
    public sealed class NotepadTriggerBehaviour : InteractableObjectBehavior
    {
        #region Fields

        [SerializeField] private NotepadTrigger[] _notepadTriggers;

        #endregion

        #region Properties

        public NotepadTrigger[] NotepadTriggers => _notepadTriggers;

        #endregion
    }
}