using System;

namespace Rescues
{
    [Serializable]
    public struct PuzzleListData
    {
        public string Name { get; set; }
        public PuzzleCondition QuestCondition { get; set; }
    }
}