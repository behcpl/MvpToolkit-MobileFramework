using Behc.Mvp.Models;
using Behc.Navigation;

namespace Features.MainPanelWithBottomBar.Models
{
    public class MainPanel
    {
        public readonly DataSlot Content;
        public readonly BottomBar BottomBar;

        private readonly NavigationManager _navigationManager;

        public MainPanel(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;

            Content = new DataSlot();
            BottomBar = new BottomBar();
            BottomBar.OnButtonClicked = ButtonClicked;
        }

        private void ButtonClicked(BottomBar.ButtonType button)
        {
            NavigationOptions options = NavigationOptions.DEFAULT;
            string feature = null;
            switch (button)
            {
                case BottomBar.ButtonType.START:
                    feature = Lobby.Models.Lobby.NAME;
                    break;
                case BottomBar.ButtonType.UNITS:
                    feature = CoreGameplay.Models.CoreGameplay.NAME;
                    options = NavigationOptions.KEEP_HISTORY;
                    break;
                case BottomBar.ButtonType.CAMPAIGN:
                    feature = Campaign.Models.Campaign.NAME;
                    break;
                case BottomBar.ButtonType.LOCKED_EXAMPLE:
                    break;
                case BottomBar.ButtonType.SOCIAL:
                    break;
            }

            if (feature != null)
            {
                _navigationManager.NavigateTo(feature, null, options);
            }
        }
    }
}