using System.Collections.Generic;


namespace Rescues
{
    public class TimeRemainingSequences
    {
        #region Fields

        public int currentSeqElementIndex;
        public List<List<ITimeRemaining>> sequentialTimeRemainings;

        #endregion


        #region ClassLifeCycles

        public TimeRemainingSequences()
        {
            sequentialTimeRemainings = new List<List<ITimeRemaining>>();
        }

        #endregion
    }
}
