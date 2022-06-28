using Behc.Utils;

namespace Features.Campaign.Models
{
    public class CampaignPanel
    {
        private readonly Campaign _campaign;
        
        public CampaignPanel(Campaign campaign)
        {
            _campaign = campaign;
        }

        public void SelectEpisode(int episodeIndex)
        {
            _campaign.OpenEpisode(episodeIndex);
        }
    }

    public class CampaignPanelFactory : IFactory<Campaign, CampaignPanel>
    {
        public CampaignPanel Create(Campaign campaign)
        {
            return new CampaignPanel(campaign);
        }
    }
}