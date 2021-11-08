using System;

namespace Rescues
{
    [Serializable]
    public struct PuzzleListData:ISavingStruct
    {
        #region Fields

        public string Name { get; set; }
        public PuzzleCondition PuzzleCondition { get; set; }

        #endregion
    }
}