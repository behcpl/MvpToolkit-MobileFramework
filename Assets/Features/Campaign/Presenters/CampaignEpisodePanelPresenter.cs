using Behc.Mvp.Presenters;
using Behc.Mvp.Utils;
using Features.Campaign.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Campaign.Presenters
{
    public class CampaignEpisodePanelPresenter : PanelBase<CampaignEpisodePanel>
    {
#pragma warning disable CS0649
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private Button _goBackButton;
        [SerializeField] private Button _fightButton;
#pragma warning restore CS0649

        protected override void OnBind(bool prepareForAnimation)
        {
            _title.text = $"Episode {_model.EpisodeIndex + 1}";
            
            DisposeOnUnbind(_goBackButton.onClick.Subscribe(_model.BackToCampaign));
            DisposeOnUnbind(_fightButton.onClick.Subscribe(() => _model.StartFight(0)));
        }
    }
}