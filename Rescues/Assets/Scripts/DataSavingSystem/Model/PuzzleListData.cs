using System;

namespace DefaultNamespace
{
    [Serializable]
    public struct PuzzleListData
    {
        public string Name { get; set; }
        public PuzzleCondition QuestCondition { get; set; }
    }
}