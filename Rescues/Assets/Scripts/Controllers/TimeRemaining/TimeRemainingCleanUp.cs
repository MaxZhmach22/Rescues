using System.Collections.Generic;


namespace Rescues
{
    public sealed class TimeRemainingCleanUp : ICleanupController
    {      
        #region Fields
                   
        private readonly List<ITimeRemaining> _timeRemainings;
        private readonly TimeRemainingSequences _sequentialTimeRemainings;

        #endregion


        #region ClassLifeCycles

        public TimeRemainingCleanUp()
        {
            _timeRemainings = TimeRemainingExtensions.TimeRemainings;
            _sequentialTimeRemainings = TimeRemainingExtensions.SequentialTimeRemainings;
        }
                   
        #endregion
        
        
        #region ICleanupController
        
        public void Cleanup()
        {
            _timeRemainings.Clear();
            _sequentialTimeRemainings.sequentialTimeRemainings.Clear();
        }

        #endregion
    }
}