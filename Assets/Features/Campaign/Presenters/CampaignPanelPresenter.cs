using Behc.Mvp.Presenters;
using Behc.Mvp.Utils;
using Features.Campaign.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Campaign.Presenters
{
    public class CampaignPanelPresenter : PanelBase<CampaignPanel>
    {
#pragma warning disable CS0649
        [SerializeField] private Button _episode1;
        [SerializeField] private Button _episode2;
        [SerializeField] private Button _episode3;
#pragma warning restore CS0649

        protected override void OnBind(bool prepareForAnimation)
        {
            DisposeOnUnbind(_episode1.onClick.Subscribe(() => _model.SelectEpisode(1)));
            DisposeOnUnbind(_episode2.onClick.Subscribe(() => _model.SelectEpisode(2)));
            DisposeOnUnbind(_episode3.onClick.Subscribe(() => _model.SelectEpisode(3)));
        }
    }
}