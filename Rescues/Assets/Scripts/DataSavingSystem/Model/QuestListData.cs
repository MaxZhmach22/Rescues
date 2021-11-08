using System;

namespace Rescues
{
    [Serializable]
    public struct QuestListData:ISavingStruct
    {
        #region Fields

        public string Name { get; set; }
        public QuestCondition QuestCondition { get; set; }

        #endregion
      
    }
}