using System;

namespace Rescues
{
    [Serializable]
    public struct QuestListData:SavingStruct
    {
        public string Name { get; set; }
        public QuestCondition QuestCondition { get; set; }
    }
}