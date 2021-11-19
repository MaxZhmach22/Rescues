using System;

namespace Rescues
{
    [Serializable]
    public struct ItemListData : ISavingStruct
    {
        #region Fields

        public string Name { get; set; }
        public ItemCondition ItemCondition { get; set; }

        #endregion
    }
}