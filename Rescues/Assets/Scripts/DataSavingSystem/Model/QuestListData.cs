using System;

namespace Rescues
{
    [Serializable]
    public struct QuestListData
    {
        public string Name { get; set; }
        public QuestCondition QuestCondition { get; set; }
    }
}