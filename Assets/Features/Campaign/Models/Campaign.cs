using Behc.Mvp.Models;
using Behc.Navigation;
using Behc.Utils;
using Features.MainPanelWithBottomBar.Models;

namespace Features.Campaign.Models
{
    public class Campaign : NavigableBase
    {
        public const string NAME = nameof(Campaign);
        
        private readonly DataSlot _mainDisplaySlot;
        private readonly MainPanel _mainPanelWithBottomBar;
        
        private readonly IFactory<Campaign, CampaignPanel> _campaignPanelFactory;
        private readonly IFactory<Campaign, int, CampaignEpisodePanel> _campaignEpisodePanelFactory;

        private CampaignPanel _campaignPanel;
        
        public Campaign(
            DataSlot mainDisplaySlot,
            MainPanel mainPanelWithBottomBar,
            IFactory<Campaign, CampaignPanel> campaignPanelFactory,
            IFactory<Campaign, int, CampaignEpisodePanel> campaignEpisodePanelFactory)
        {
            _mainDisplaySlot = mainDisplaySlot;
            _mainPanelWithBottomBar = mainPanelWithBottomBar;
            _campaignPanelFactory = campaignPanelFactory;
            _campaignEpisodePanelFactory = campaignEpisodePanelFactory;
        }

        protected override void OnStart()
        {
            OpenCampaign();
        }

        public void OpenEpisode(int episodeIndex)
        {
            CampaignEpisodePanel episodePanel = _campaignEpisodePanelFactory.Create(this, episodeIndex);
            _mainDisplaySlot.Data = episodePanel;
        }

        public void OpenCampaign()
        {
            _campaignPanel ??= _campaignPanelFactory.Create(this);
            
            _mainDisplaySlot.Data = _mainPanelWithBottomBar;
            _mainPanelWithBottomBar.Content.Data = _campaignPanel;
            _mainPanelWithBottomBar.BottomBar.Select(BottomBar.ButtonType.CAMPAIGN);
        }

        public void StartFight(int episodeIndex, int fightIndex)
        {
            //TODO:
        }
    }

    public class CampaignFactory : IFactory<object, INavigable>
    {
        private readonly DataSlot _mainDisplaySlot;
        private readonly MainPanel _mainPanelWithBottomBar;
        private readonly CampaignPanelFactory _campaignPanelFactory;
        private readonly CampaignEpisodePanelFactory _campaignEpisodePanelFactory;

        public CampaignFactory(
            DataSlot mainDisplaySlot,
            MainPanel mainPanelWithBottomBar,
            CampaignPanelFactory campaignPanelFactory,
            CampaignEpisodePanelFactory campaignEpisodePanelFactory)
        {
            _mainDisplaySlot = mainDisplaySlot;
            _mainPanelWithBottomBar = mainPanelWithBottomBar;
            _campaignPanelFactory = campaignPanelFactory;
            _campaignEpisodePanelFactory = campaignEpisodePanelFactory;
        }

        public INavigable Create(object parameters)
        {
            return new Campaign(_mainDisplaySlot, _mainPanelWithBottomBar, _campaignPanelFactory, _campaignEpisodePanelFactory);
        }
    }
}