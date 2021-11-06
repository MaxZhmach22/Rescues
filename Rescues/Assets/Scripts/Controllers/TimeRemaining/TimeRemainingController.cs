using System.Collections.Generic;


namespace Rescues
{
    public sealed class TimeRemainingController : IExecuteController
    {
        #region Fields

        private readonly List<ITimeRemaining> _timeRemainings;
        private readonly List<List<ITimeRemaining>> _sequentialTimeRemainings;
        private readonly UnityTimeServices _timeService;
        private int _currentSequenceIndex = 0;
        private int _currentSeqElementIndex = 0;

        #endregion


        #region ClassLifeCycles

        public TimeRemainingController()
        {
            _timeRemainings = TimeRemainingExtensions.TimeRemainings;
            _sequentialTimeRemainings = TimeRemainingExtensions.SequentialTimeRemainings;
            _timeService = Services.SharedInstance.UnityTimeServices;
        }

        #endregion


        #region IExecuteController

        public void Execute()
        {
            var time = _timeService.DeltaTime();
            for (var i = 0; i < _timeRemainings.Count; i++)
            {
                var obj = _timeRemainings[i];
                obj.CurrentTime -= time;
                if (obj.CurrentTime <= 0.0f)
                {
                    obj?.Method?.Invoke();
                    if (!obj.IsRepeating)
                    {
                        obj.RemoveTimeRemaining();
                    }
                    else
                    {
                        obj.CurrentTime = obj.Time;
                    }
                }
            }

            if (_sequentialTimeRemainings.Count > _currentSequenceIndex &&
                _sequentialTimeRemainings[_currentSequenceIndex].Count > _currentSeqElementIndex)
            {
                var sequenceElement = _sequentialTimeRemainings[_currentSequenceIndex][_currentSeqElementIndex];
                sequenceElement.CurrentTime -= time;
                if (sequenceElement.CurrentTime <= 0.0f)
                {
                    sequenceElement?.Method?.Invoke();
                    _currentSeqElementIndex++;
                }

                if (_currentSeqElementIndex >= _sequentialTimeRemainings[_currentSequenceIndex].Count)
                {
                    _sequentialTimeRemainings[_currentSequenceIndex].RemoveSequentialTimeRemaining();
                    _currentSequenceIndex++;
                }
            }
            else
            {
                _currentSequenceIndex = 0;
                _currentSeqElementIndex = 0;
            }
        }

        #endregion
    }
}
