using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Rescues
{
    public sealed class NotepadTextContent
    {
        #region Fields

        private NotepadData _notepadData;
        private string DATA_PATH = "Data/Notepad";
        private string STRIKE_OPEN = "<s>";
        private string STRIKE_CLOSE = "</s>";
        private string UNDER_OPEN = "<u>";
        private string UNDER_CLOSE = "</u>";
        private string TAB = "    ";

        #endregion

        #region ClassLifeCycles

        public NotepadTextContent()
        {
            var notepadData = Resources.LoadAll<NotepadData>(DATA_PATH);

            if (notepadData.Length == 0)
                throw new System.Exception("Файл NotepadData должен быть создан в папке Resources/Data/Notepad");

            _notepadData = notepadData[0];
        }

        #endregion

        #region Methods

        public string GetTextToDisplay(NoteCategory category, List<NotepadEntry> entries)
        {
            var rawJson = GetTextToDeserialize(category);

            var jo = JObject.Parse(rawJson);

            StringBuilder sb = new StringBuilder();

            foreach (var entry in entries)
            {
                if (jo[entry.EntryName] == null)
                    continue;

                sb.Append(UNDER_OPEN +
                    (entry.IsCrossedOut ? StrikeThroughText(entry.EntryName) : entry.EntryName) +
                    UNDER_CLOSE);

                var bpTextCount = 0;
                foreach (var value in jo[entry.EntryName].Values())
                    bpTextCount++;

                foreach (var bulletPoint in entry.BulletPoints)
                {
                    if (bulletPoint.Id >= bpTextCount)
                        continue;

                    var bpText = jo[entry.EntryName][bulletPoint.Id].Value<string>();

                    sb.Append("\n" + TAB +
                        (bulletPoint.IsCrossedOut ? StrikeThroughText(bpText) : bpText));

                }

                sb.Append("\n\n");
            }

            return sb.ToString();

            string StrikeThroughText(string text)
            {
                return STRIKE_OPEN + text + STRIKE_CLOSE;
            }
        }

        private string GetTextToDeserialize(NoteCategory category)
        {
            switch (category)
            {
                case NoteCategory.Character:
                    return _notepadData.Characters;
                case NoteCategory.Dialogue:
                    return _notepadData.Dialogues;
                case NoteCategory.Lore:
                    return _notepadData.Lore;
                case NoteCategory.Quest:
                    return _notepadData.Quests;
                default:
                    return string.Empty;
            }
        }

        #endregion
    }
}