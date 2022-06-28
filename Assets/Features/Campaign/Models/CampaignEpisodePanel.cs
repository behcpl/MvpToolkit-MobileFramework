using Behc.Utils;

namespace Features.Campaign.Models
{
    public class CampaignEpisodePanel
    {
        public int EpisodeIndex { get; }

        private readonly Campaign _campaign;

        public CampaignEpisodePanel(Campaign campaign, int episodeIndex)
        {
            _campaign = campaign;
            EpisodeIndex = episodeIndex;
        }

        public void BackToCampaign()
        {
            _campaign.OpenCampaign();
        }

        public void StartFight(int fightIndex)
        {
            _campaign.StartFight(EpisodeIndex, fightIndex);
        }
    }

    public class CampaignEpisodePanelFactory : IFactory<Campaign, int, CampaignEpisodePanel>
    {
        public CampaignEpisodePanel Create(Campaign campaign, int episodeIndex)
        {
            return new CampaignEpisodePanel(campaign, episodeIndex);
        }
    }
}