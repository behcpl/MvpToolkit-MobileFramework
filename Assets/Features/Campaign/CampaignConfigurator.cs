using Application.Root;
using Behc.Configuration;
using Behc.Mvp.Models;
using Behc.Mvp.Presenters;
using Behc.Mvp.Presenters.Factories;
using Behc.Navigation;
using Features.Campaign.Models;
using Features.Campaign.Presenters;
using Features.MainPanelWithBottomBar.Models;
using UnityEngine;

namespace Features.Campaign
{
    [CreateAssetMenu(fileName = "CampaignConfigurator", menuName = "Game/Configurator/Campaign", order = 0)]
    public class CampaignConfigurator : ScriptableConfigurator
    {
#pragma warning disable CS0649
        [SerializeField] private CampaignPanelPresenter _campaignPanelPresenter;
        [SerializeField] private CampaignEpisodePanelPresenter _campaignEpisodePanelPresenter;
#pragma warning restore CS0649

        protected override void OnLoad(IDependencyResolver resolver)
        {
            PresenterUpdateKernel kernel = resolver.Resolve<PresenterUpdateKernel>();
            PresenterMap globalPresenterMap = resolver.Resolve<PresenterMap>();
            INavigationRegistry navigationRegistry = resolver.Resolve<INavigationRegistry>();
         
            DataSlot mainDisplaySlot = resolver.Resolve<DataSlot>(RootElements.MAIN_DISPLAY_SLOT);
            MainPanel mainPanelWithBottomBar = resolver.Resolve<MainPanel>();
     
            DisposeOnUnload(globalPresenterMap.RegisterFactory<CampaignPanel>(_campaignPanelPresenter.gameObject, kernel));
            DisposeOnUnload(globalPresenterMap.RegisterFactory<CampaignEpisodePanel>(_campaignEpisodePanelPresenter.gameObject, kernel));

            CampaignPanelFactory campaignPanelFactory = new CampaignPanelFactory();
            CampaignEpisodePanelFactory campaignEpisodePanelFactory = new CampaignEpisodePanelFactory();
            CampaignFactory campaignFactory = new CampaignFactory(mainDisplaySlot, mainPanelWithBottomBar, campaignPanelFactory, campaignEpisodePanelFactory);
            
            DisposeOnUnload(navigationRegistry.Register(Campaign.Models.Campaign.NAME, campaignFactory));
        }
    }
}