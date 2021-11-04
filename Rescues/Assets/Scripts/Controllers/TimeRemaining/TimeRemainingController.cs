using System.Collections.Generic;


namespace Rescues
{
    public sealed class TimeRemainingController : IExecuteController
    {
        #region Fields

        private readonly List<ITimeRemaining> _timeRemainings;
        private readonly List<List<ITimeRemaining>> _sequentialTimeRemainings;
        private readonly UnityTimeServices _timeService;
        private int k = 0;
        private int j = 0;

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

            if (_sequentialTimeRemainings.Count > k && _sequentialTimeRemainings[k].Count > j)
            {
                var sequence = _sequentialTimeRemainings[k][j];
                sequence.CurrentTime -= time;
                if (sequence.CurrentTime <= 0.0f)
                {
                    sequence?.Method?.Invoke();
                    j++;
                }
                if (j >= _sequentialTimeRemainings[k].Count)
                {
                    _sequentialTimeRemainings[k].RemoveSequentialTimeRemaining();
                    j = 0;
                    k++;
                }
                if (k >= _sequentialTimeRemainings.Count)
                {
                    k = 0;
                }
            }
            else
            {
                k = 0;
                j = 0;
            }
        }

        #endregion
    }
}
