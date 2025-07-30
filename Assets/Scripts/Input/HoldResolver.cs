using SLS.Core.Timers;
using System;
using UnityEngine.Events;

namespace GMTK2025.Inputs
{
    public class HoldResolver : IHoldResolver, IDisposable
    {
        public struct Config
        {
            public float HoldThresholdSeconds;
        }

        public static Config DefaultConfig = default;
        private ITimer timer = default;
        private Config config = default;

        public event UnityAction OnHoldStart;
        public event UnityAction OnHoldEnd;
        public event UnityAction OnClick;
        public event UnityAction<float> OnHoldProgressChanged;

        private ITimer Timer
        {
            get
            {
                if (timer is null)
                {
                    TimeSpan threshold = TimeSpan.FromSeconds(config.HoldThresholdSeconds);
                    timer = SLS.Core.Timers.Timer.CreateUnscaledTimer(threshold);
                    timer.OnTick.AddListener(UpdateHoldPercent);
                    timer.OnEnd.AddListener(OnHoldTimeElapsed);
                    timer.AutoRestart = false;
                }
                return timer;
            }
        }

        public HoldResolver()
        {
            this.config = new Config { HoldThresholdSeconds = 0.4f };
        }

        public HoldResolver(Config config)
        {
            this.config = config;
        }

        public void ReportHoldInputDown()
        {
            Timer.Restart();
        }

        public void ReportHoldInputUp()
        {
            if (Timer.IsEnded)
            {
                OnHoldEnd?.Invoke();
            }
            else
            {
                Timer.Stop();
                OnClick?.Invoke();
            }
        }

        public void DoHoldOverride()
        {
            Timer.Stop();
            OnHoldStart?.Invoke();
        }

        public void OnClickOverride()
        {
            Timer.Stop();
            OnClick?.Invoke();
        }

        public void OnHoldTimeElapsed()
        {
            Timer.Stop();
            OnHoldStart?.Invoke();
        }

        private void UpdateHoldPercent(TimeSpan _)
        {
            double currentPercent = timer.Elapsed.TotalSeconds / timer.Interval.TotalSeconds;
            OnHoldProgressChanged?.Invoke((float)currentPercent);
        }

        public void Dispose()
        {
            timer?.Dispose();
            timer = null;
        }
    }
}