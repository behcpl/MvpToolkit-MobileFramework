using Behc.Configuration;
using Behc.Navigation;
using Behc.Utils;
using Features.Loader.Models;
using Features.Lobby.Models;

namespace Application
{
    public class ApplicationMain
    {
        private const string _LOADER_MESSAGE = "Loading...";

        private readonly NavigationManager _navigationManager;
        private readonly LoaderPanelHelper _loaderPanelHelper;
        private readonly ConfiguratorSet _defaultConfiguratorSet;

        public ApplicationMain(
            NavigationManager navigationManager,
            LoaderPanelHelper loaderPanelHelper,
            ConfiguratorSet defaultConfiguratorSet)
        {
            _navigationManager = navigationManager;
            _loaderPanelHelper = loaderPanelHelper;
            _defaultConfiguratorSet = defaultConfiguratorSet;
        }

        public void Start()
        {
            WhenBoth whenBoth = new WhenBoth();
            whenBoth.Setup(LoadingFinished);

            _loaderPanelHelper.Setup(
                _LOADER_MESSAGE,
                null,
                panel => panel.Advance(_LOADER_MESSAGE, _defaultConfiguratorSet.GetLoadingProgress()),
                panel => whenBoth.CompletedSecond());

            _defaultConfiguratorSet.Load(() => whenBoth.CompletedFirst());
        }

        private void LoadingFinished()
        {
            _navigationManager.NavigateTo(Lobby.NAME, null, NavigationOptions.DEFAULT);
        }
    }
}