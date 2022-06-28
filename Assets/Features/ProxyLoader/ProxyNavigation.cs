using System;
using Behc.Configuration;
using Behc.Navigation;
using Behc.Utils;
using Features.Loader.Models;

namespace Features.ProxyLoader
{
    public class ProxyNavigation : INavigable
    {
        private const string _LOADING_MESSAGE = "Loading assets...";
        private const string _UNLOADING_MESSAGE = "Unloading assets...";

        private readonly ProxyNavigationRegistry _proxyNavigationRegistry;
        private readonly ConfiguratorSet _configuratorSet;
        private readonly LoaderPanelHelper _loaderPanelHelper;

        private object _parameters;
        private INavigable _navigable;
        private bool _running;

        public ProxyNavigation(
            ProxyNavigationRegistry proxyNavigationRegistry,
            ConfiguratorSet configuratorSet,
            LoaderPanelHelper loaderPanelHelper,
            object parameters)
        {
            _proxyNavigationRegistry = proxyNavigationRegistry;
            _configuratorSet = configuratorSet;
            _loaderPanelHelper = loaderPanelHelper;
            _parameters = parameters;

            WhenBoth whenBoth = new WhenBoth();
            whenBoth.Setup(InnerFeatureLoaded);

            if (_configuratorSet.Status == ConfiguratorStatus.UNLOADED)
            {
                _loaderPanelHelper.Setup(_LOADING_MESSAGE, null, PanelUpdate, panel => whenBoth.CompletedSecond());
                _configuratorSet.Load(() => whenBoth.CompletedFirst());
            }

            void PanelUpdate(LoaderPanel panel)
            {
                panel.Advance(_LOADING_MESSAGE, _configuratorSet.GetLoadingProgress());
            }
        }

        void INavigable.Start()
        {
            _running = true;
            _navigable?.Start();
        }

        void INavigable.Restart(object parameters)
        {
            _parameters = parameters;
            _navigable?.Restart(parameters);
        }

        void INavigable.Stop()
        {
            _running = false;
            _navigable?.Stop();
        }

        void INavigable.LongDispose(Action onComplete)
        {
            //TODO: still loading scenario: wait to finish, then start unloading

            WhenBoth whenBoth = new WhenBoth();
            whenBoth.Setup(FinishDispose);

            _navigable.LongDispose(InnerFeatureDone);

            void InnerFeatureDone()
            {
                _loaderPanelHelper.Setup(_UNLOADING_MESSAGE, PanelReady, PanelUpdate, panel => whenBoth.CompletedSecond());
            }

            void PanelReady(LoaderPanel panel)
            {
                _configuratorSet.Unload(() => whenBoth.CompletedFirst());
            }

            void PanelUpdate(LoaderPanel panel)
            {
                panel.Advance(_UNLOADING_MESSAGE, _configuratorSet.GetUnloadingProgress());
            }

            void FinishDispose()
            {
                _navigable = null;
                _parameters = null;
                onComplete.Invoke();
            }
        }

        private void InnerFeatureLoaded()
        {
            _navigable = _proxyNavigationRegistry.Create(_parameters);
            if (_running)
            {
                _navigable.Start();
            }
        }
    }

    public class ProxyNavigationFactory : IFactory<object, INavigable>
    {
        private readonly ProxyNavigationRegistry _proxyNavigationRegistry;
        private readonly ConfiguratorSet _configuratorSet;
        private readonly LoaderPanelHelper _loaderPanelHelper;

        public ProxyNavigationFactory(
            ProxyNavigationRegistry proxyNavigationRegistry,
            ConfiguratorSet configuratorSet,
            LoaderPanelHelper loaderPanelHelper)
        {
            _proxyNavigationRegistry = proxyNavigationRegistry;
            _configuratorSet = configuratorSet;
            _loaderPanelHelper = loaderPanelHelper;
        }

        public INavigable Create(object parameters)
        {
            return new ProxyNavigation(_proxyNavigationRegistry, _configuratorSet, _loaderPanelHelper, parameters);
        }
    }
}