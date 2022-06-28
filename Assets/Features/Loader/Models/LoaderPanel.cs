using System;
using Behc.Mvp.Models;
using Behc.Utils;

namespace Features.Loader.Models
{
    public class LoaderPanel : ReactiveModel
    {
        public float Progress { get; private set; }
        public string Message { get; private set; }

        private readonly Action<LoaderPanel> _onReady;
        private readonly Action<LoaderPanel> _onPoll;
        private readonly Action<LoaderPanel> _onElapsedMinimalTime;

        private bool _elapsedMinimalTime;
        
        public LoaderPanel(Action<LoaderPanel> onReady, Action<LoaderPanel> onPoll, Action<LoaderPanel> onElapsedMinimalTime)
        {
            _onReady = onReady;
            _onPoll = onPoll;
            _onElapsedMinimalTime = onElapsedMinimalTime;
        }

        public void Advance(string message, float progress)
        {
            Progress = progress;
            Message = message;
            NotifyChanges();
        }

        public void Ready()
        {
            _onReady?.Invoke(this);
        }

        public void PollData()
        {
            _onPoll?.Invoke(this);
        }
        
        public void ElapsedMinimalTime()
        {
            if (!_elapsedMinimalTime)
            {
                _elapsedMinimalTime = true;
                _onElapsedMinimalTime?.Invoke(this);
            }
        }
    }

    public class LoaderPanelFactory : IFactory<Action<LoaderPanel>, Action<LoaderPanel>, Action<LoaderPanel>, LoaderPanel>
    {
        public LoaderPanel Create(Action<LoaderPanel> onReady, Action<LoaderPanel> onPoll, Action<LoaderPanel> onElapsedMinimalTime)
        {
            return new LoaderPanel(onReady, onPoll, onElapsedMinimalTime);
        }
    }
}