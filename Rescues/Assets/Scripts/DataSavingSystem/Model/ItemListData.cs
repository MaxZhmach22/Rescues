using System;

namespace Rescues
{
    [Serializable]
    public struct ItemListData : SavingStruct
    {
        public string Name { get; set; }
        public ItemCondition ItemCondition { get; set; }
    }

    
}