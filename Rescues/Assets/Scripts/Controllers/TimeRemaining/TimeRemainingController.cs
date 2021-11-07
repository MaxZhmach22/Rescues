using System.Collections.Generic;


namespace Rescues
{
    public sealed class TimeRemainingController : IExecuteController
    {
        #region Fields

        private readonly List<ITimeRemaining> _timeRemainings;
        private readonly TimeRemainingSequences _timeRemainingSequences;
        private readonly UnityTimeServices _timeService;

        #endregion


        #region ClassLifeCycles

        public TimeRemainingController()
        {
            _timeRemainings = TimeRemainingExtensions.TimeRemainings;
            _timeRemainingSequences = TimeRemainingExtensions.SequentialTimeRemainings;
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

            if (_timeRemainingSequences.currentSequenceIndex >= _timeRemainingSequences.sequentialTimeRemainings.Count)
            {
                _timeRemainingSequences.currentSequenceIndex = 0;
            }
            else
            {
                var sequences = _timeRemainingSequences.sequentialTimeRemainings;
                var sequenceElement = sequences[_timeRemainingSequences.currentSequenceIndex]
                    [_timeRemainingSequences.currentSeqElementIndex];
                sequenceElement.CurrentTime -= time;
                if (sequenceElement.CurrentTime <= 0.0f)
                {
                    sequenceElement?.Method?.Invoke();
                    _timeRemainingSequences.currentSeqElementIndex++;
                }

                if (_timeRemainingSequences.currentSeqElementIndex >= sequences[_timeRemainingSequences.
                    currentSequenceIndex].Count)
                {
                    sequences[_timeRemainingSequences.
                    currentSequenceIndex].RemoveSequentialTimeRemaining();
                    _timeRemainingSequences.currentSequenceIndex++;
                }
            }

        }

        #endregion
    }
}
