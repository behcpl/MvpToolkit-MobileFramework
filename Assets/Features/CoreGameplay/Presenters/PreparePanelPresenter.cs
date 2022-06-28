using System;
using Behc.MiniTween;
using Behc.MiniTween.Extensions;
using Behc.Mvp.Presenters;
using Behc.Mvp.Utils;
using Behc.Utils;
using Features.CoreGameplay.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Features.CoreGameplay.Presenters
{
    public class PreparePanelPresenter : PanelBase<PreparePanel>
    {
#pragma warning disable CS0649
        [SerializeField] private Button _goBackButton;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private AbstractProvider<ITweenSystem> _miniTweenProvider;
#pragma warning restore CS0649

        protected override void OnBind(bool prepareForAnimation)
        {
            Debug.Log("PreparePanelPresenter::OnBind");
            DisposeOnUnbind(_goBackButton.onClick.Subscribe(_model.GoBack));
            // _canvasGroup.alpha = prepareForAnimation ? 0 : 1;
        }

        // protected override void OnAnimateShow(float startTime, Action onFinish)
        // {
        //     _canvasGroup.AnimateAlpha( _miniTweenProvider.GetInstance(), 1.0f, 0.3f).SetCompleteCallback(onFinish);
        // }
    }
}