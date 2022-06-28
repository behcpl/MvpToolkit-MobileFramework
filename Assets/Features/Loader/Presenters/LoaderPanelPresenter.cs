using System;
using Behc.Mvp.Presenters;
using Features.Loader.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Loader.Presenters
{
    public class LoaderPanelPresenter : PanelBase<LoaderPanel>
    {
#pragma warning disable CS0649
        [SerializeField] private Image _progressBar;
        [SerializeField] private TextMeshProUGUI _progressValue;
        [SerializeField] private TextMeshProUGUI _message;

        [SerializeField] private float _minimalTime = 0.0f;
#pragma warning restore CS0649

        private bool _readyToPoll;
        private float _timeCounter;

        protected override void OnBind(bool prepareForAnimation)
        {
            DisposeOnUnbind(_model.Subscribe(Refresh));

            //update data at start
            _timeCounter = 0;
            Refresh();

            if (!prepareForAnimation)
            {
                _readyToPoll = true;
                _model.Ready();
            }
        }

        protected override void OnUnbind()
        {
            _readyToPoll = false;
        }

        protected override void OnAnimateShow(float startTime, Action onFinish)
        {
            _readyToPoll = true;
            _model.Ready();
            onFinish?.Invoke();
        }

        private void Refresh()
        {
            float progress = _model.Progress;
            if (_minimalTime > 0)
            {
                progress = Mathf.Min(progress, _timeCounter / _minimalTime);
            }
            
            _message.text = _model.Message;
            _progressBar.fillAmount = progress; //TODO: optimize, no need to set every frame or fraction of progress
            _progressValue.text = $"{Mathf.RoundToInt(progress * 100)}%";
        }

        private void Update()
        {
            if (_readyToPoll && _model != null)
            {
                _model.PollData();
                _timeCounter += Time.unscaledDeltaTime;

                if (_timeCounter > _minimalTime)
                {
                    _model.ElapsedMinimalTime();
                }
            }
        }
    }
}