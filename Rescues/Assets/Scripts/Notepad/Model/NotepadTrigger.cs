using System;
using UnityEngine;


namespace Rescues
{
    [Serializable]
    public struct NotepadTrigger
    {
        #region Fields

        [SerializeField] private NoteCategory _category;
        [SerializeField] private string _entryName;
        [SerializeField, Min(-1)] private int _bulletpointID;
        [SerializeField] private NotepadEntryAction _action;

        #endregion


        #region Properties

        public NoteCategory Category => _category;
        public string EntryName => _entryName;
        public int BulletpointID => _bulletpointID;
        public NotepadEntryAction Action => _action;

        #endregion
    }
}