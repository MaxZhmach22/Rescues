using UnityEngine;
using System;

namespace Rescues
{
    [CreateAssetMenu(fileName = "NotepadData", menuName = "Data/Notepad/NotepadData")]
    public sealed class NotepadData : ScriptableObject
    {
        #region Fields

        [SerializeField] private TextAsset _quests;
        [SerializeField] private TextAsset _dialogues;
        [SerializeField] private TextAsset _characters;
        [SerializeField] private TextAsset _lore;

        #endregion

        #region Properties

        public string Quests
        {
            get 
            {
                if (_quests == null)
                    throw new Exception("Текстовый файл Quests должен быть указан в NotepadData");

                return _quests.text;
            }  
        }

        public string Dialogues
        {
            get
            {
                if (_dialogues == null)
                    throw new Exception("Текстовый файл Dialogues должен быть указан в NotepadData");

                return _dialogues.text;
            }
        }


        public string Characters
        {
            get
            {
                if (_characters == null)
                    throw new Exception("Текстовый файл Characters должен быть указан в NotepadData");

                return _characters.text;
            }
        }

        public string Lore
        {
            get
            {
                if (_lore == null)
                    throw new Exception("Текстовый файл Lore должен быть указан в NotepadData");

                return _lore.text;
            }
        }

        #endregion
    }
}