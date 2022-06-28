using Application.Root;
using Behc.Configuration;
using Behc.Mvp.Models;
using Behc.Mvp.Presenters;
using Behc.Mvp.Presenters.Factories;
using Behc.Navigation;
using Features.Lobby.Models;
using Features.Lobby.Presenters;
using Features.MainPanelWithBottomBar.Models;
using UnityEngine;

namespace Features.Lobby
{
    [CreateAssetMenu(fileName = "LobbyConfigurator", menuName = "Game/Configurator/Lobby", order = 0)]
    public class LobbyConfigurator : ScriptableConfigurator
    {
#pragma warning disable CS0649
        [SerializeField] private LobbyPanelPresenter _lobbyPanelPresenter;
#pragma warning restore CS0649

        protected override void OnLoad(IDependencyResolver resolver)
        {
            PresenterUpdateKernel kernel = resolver.Resolve<PresenterUpdateKernel>();
            PresenterMap globalPresenterMap = resolver.Resolve<PresenterMap>();
            INavigationRegistry navigationRegistry = resolver.Resolve<INavigationRegistry>();
         
            DataSlot mainDisplaySlot = resolver.Resolve<DataSlot>(RootElements.MAIN_DISPLAY_SLOT);
            MainPanel mainPanelWithBottomBar = resolver.Resolve<MainPanel>();

            DisposeOnUnload(globalPresenterMap.RegisterFactory<LobbyPanel>(_lobbyPanelPresenter.gameObject, kernel));

            LobbyPanelFactory lobbyPanelFactory = new LobbyPanelFactory();
            LobbyFactory lobbyFactory = new LobbyFactory(mainDisplaySlot, mainPanelWithBottomBar, lobbyPanelFactory);

            DisposeOnUnload(navigationRegistry.Register(Lobby.Models.Lobby.NAME, lobbyFactory));
        }
    }
}