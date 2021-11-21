using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Rescues
{
    public sealed class NotepadBehaviour : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Button _leftButton;
        [SerializeField] private Button _rightButton;
        [SerializeField] private Button _questsBookmark;
        [SerializeField] private Button _dialoguesBookmark;
        [SerializeField] private Button _charactersBookmark;
        [SerializeField] private Button _loreBookmark;
        [SerializeField] private TextMeshProUGUI _notepadText;

        private NotepadTextContent _notepadTextContent;

        private List<NotepadEntry> _quests = new List<NotepadEntry>();
        private List<NotepadEntry> _dialogues = new List<NotepadEntry>();
        private List<NotepadEntry> _characters = new List<NotepadEntry>();
        private List<NotepadEntry> _lore = new List<NotepadEntry>();

        private List<NotepadEntry> _currentCategory;

        private NoteCategory _lastViewedCategory;

        #endregion

        #region Methods

        public void Initialize()
        {
            _notepadTextContent = new NotepadTextContent();

            _questsBookmark.onClick.AddListener(() => DisplayText(NoteCategory.Quest));
            _dialoguesBookmark.onClick.AddListener(() => DisplayText(NoteCategory.Dialogue));
            _charactersBookmark.onClick.AddListener(() => DisplayText(NoteCategory.Character));
            _loreBookmark.onClick.AddListener(() => DisplayText(NoteCategory.Lore));

            _leftButton.onClick.AddListener(() => TurnThePage(false));
            _rightButton.onClick.AddListener(() => TurnThePage(true));

            _quests.Add(new NotepadEntry {
                EntryName = "Заселиться в отель",
                IsCrossedOut = false,
            BulletPoints = new List<NotepadBulletpoint> 
                { 
                    new NotepadBulletpoint { Id = 0, IsCrossedOut = false }
                }
            }
                );
        }

        public void ProcessTrigger(NotepadTrigger entry)
        {
            switch (entry.Action)
            {
                case NotepadEntryAction.Add:
                    {
                        AddEntry(entry.Category, entry.EntryName, entry.BulletpointID);
                        break;
                    }
                case NotepadEntryAction.CrossOut:
                    {
                        CrossOutEntry(entry.Category, entry.EntryName, entry.BulletpointID);
                        break;
                    }
                case NotepadEntryAction.Remove:
                    {
                        RemoveEntry(entry.Category, entry.EntryName, entry.BulletpointID);
                        break;
                    }
                default:
                    {
                        throw new System.Exception("Компонент NotepadTriggerBehaviour на объекте должен иметь настроенное поле Category");
                    }
            }
        }

        private void AddEntry(NoteCategory category, string entryName, int bulletpointId)
        {
            _currentCategory = SelectCategory(category);

            var entryIndex = _currentCategory.FindIndex(x => x.EntryName.Equals(entryName));

            if (entryIndex < 0)
            {

                _currentCategory.Add(new NotepadEntry
                {
                    EntryName = entryName,
                    BulletPoints = new List<NotepadBulletpoint> { 
                        new NotepadBulletpoint { Id = bulletpointId < 0 ? 0 : bulletpointId
                        } 
                    }
                });
            }
            else
            {
                _currentCategory[entryIndex].AddBulletpoint(bulletpointId);
            }
        }

        private void RemoveEntry(NoteCategory category, string entryName, int bulletpointId)
        {
            _currentCategory = SelectCategory(category);

            var entryIndex = _currentCategory.FindIndex(x => x.EntryName.Equals(entryName));

            if (entryIndex < 0)
                return;

            if (bulletpointId >= 0)
                _currentCategory[entryIndex].RemoveBulletpoint(bulletpointId);
            else
                _currentCategory.RemoveAt(entryIndex);
        }

        private void CrossOutEntry(NoteCategory category, string entryName, int bulletpointId)
        {
            _currentCategory = SelectCategory(category);

            var entryIndex = _currentCategory.FindIndex(x => x.EntryName.Equals(entryName));

            if (entryIndex < 0)
                return;

            if (bulletpointId >= 0)
                _currentCategory[entryIndex].CrossOutBulletpoint(bulletpointId);
            else
                _currentCategory[entryIndex].CrossOutEntry();
        }

        private List<NotepadEntry> SelectCategory(NoteCategory category)
        {
            switch (category)
            {
                case NoteCategory.Quest:
                    {
                        return _quests;
                    }
                case NoteCategory.Dialogue:
                    {
                        return _dialogues;
                    }
                case NoteCategory.Character:
                    {
                        return _characters;
                    }
                case NoteCategory.Lore:
                    {
                        return _lore;
                    }
                default:
                    {
                        throw new System.Exception("Компонент NotepadTriggerBehaviour должен иметь настроенное поле Category");
                    }
            }
        }

        private void DisplayText(NoteCategory category)
        {
            if (category == NoteCategory.None)
                category = NoteCategory.Quest;

            _notepadText.text = _notepadTextContent.GetTextToDisplay(category, SelectCategory(category));
            _notepadText.pageToDisplay = 1;

            _lastViewedCategory = category;
        }

        public void ShowPageButtons()
        {
            var isManyPages = _notepadText.textInfo.pageCount > 1;

            _leftButton.gameObject.SetActive(isManyPages);
            _rightButton.gameObject.SetActive(isManyPages);
        }

        private void TurnThePage(bool doIncrease)
        {
            if (doIncrease && _notepadText.pageToDisplay < _notepadText.textInfo.pageCount)
                _notepadText.pageToDisplay++;

            if (!doIncrease && _notepadText.pageToDisplay > 1)
                _notepadText.pageToDisplay--;
        }

        #endregion

        #region UnityMethods

        private void OnEnable()
        {
            DisplayText(_lastViewedCategory);
        }

        #endregion
    }
}
