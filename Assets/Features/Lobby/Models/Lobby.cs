using Behc.Mvp.Models;
using Behc.Navigation;
using Behc.Utils;
using Features.MainPanelWithBottomBar.Models;

namespace Features.Lobby.Models
{
    public class Lobby : NavigableBase
    {
        public const string NAME = nameof(Lobby);

        private readonly DataSlot _fullscreenPanels;
        private readonly MainPanel _mainPanelWithBottomBar;

        private readonly LobbyPanel _lobbyPanel;

        public Lobby(
            DataSlot fullscreenPanels,
            MainPanel mainPanelWithBottomBar,
            LobbyPanelFactory lobbyPanelFactory)
        {
            _fullscreenPanels = fullscreenPanels;
            _mainPanelWithBottomBar = mainPanelWithBottomBar;

            _lobbyPanel = lobbyPanelFactory.Create();
        }

        protected override void OnStart()
        {
            _fullscreenPanels.Data = _mainPanelWithBottomBar;
            _mainPanelWithBottomBar.Content.Data = _lobbyPanel;
            _mainPanelWithBottomBar.BottomBar.Select(BottomBar.ButtonType.START);
        }
    }

    public class LobbyFactory : IFactory<object, INavigable>
    {
        private readonly DataSlot _fullscreenPanels;
        private readonly MainPanel _mainPanelWithBottomBar;
        private readonly LobbyPanelFactory _lobbyPanelFactory;

        public LobbyFactory(
            DataSlot fullscreenPanels,
            MainPanel mainPanelWithBottomBar,
            LobbyPanelFactory lobbyPanelFactory)
        {
            _fullscreenPanels = fullscreenPanels;
            _mainPanelWithBottomBar = mainPanelWithBottomBar;
            _lobbyPanelFactory = lobbyPanelFactory;
        }

        public INavigable Create(object parameters)
        {
            return new Lobby(_fullscreenPanels, _mainPanelWithBottomBar, _lobbyPanelFactory);
        }
    }
}