using System.Collections.Generic;


namespace Rescues
{
    public sealed class TimeRemainingCleanUp : ICleanupController
    {      
        #region Fields
                   
        private readonly List<ITimeRemaining> _timeRemainings;
        private readonly List<List<ITimeRemaining>> _sequentialЕimeRemainings;

        #endregion


        #region ClassLifeCycles

        public TimeRemainingCleanUp()
        {
            _timeRemainings = TimeRemainingExtensions.TimeRemainings;
            _sequentialЕimeRemainings = TimeRemainingExtensions.SequentialTimeRemainings;
        }
                   
        #endregion
        
        
        #region ICleanupController
        
        public void Cleanup()
        {
            _timeRemainings.Clear();
            _sequentialЕimeRemainings.Clear();
        }

        #endregion
    }
}