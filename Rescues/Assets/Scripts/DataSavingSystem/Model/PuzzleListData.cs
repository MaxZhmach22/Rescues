using System;

namespace Rescues
{
    [Serializable]
    public struct PuzzleListData:SavingStruct
    {
        public string Name { get; set; }
        public PuzzleCondition PuzzleCondition { get; set; }
    }
}