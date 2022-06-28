using Behc.Configuration;
using Behc.Configuration.Loaders;
using Behc.Navigation;
using Behc.Utils;
using Features.Loader.Models;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Features.ProxyLoader
{
    [CreateAssetMenu(fileName = "ProxyConfigurator", menuName = "Game/Configurator/ProxyConfigurator", order = 0)]
    public class ProxyConfigurator : ScriptableConfigurator
    {
        [SerializeField] private string _navigationName = "EnterNameHere";
        [SerializeField] private AssetReferenceT<ScriptableConfigurator> _configurator;

        protected override void OnLoad(IDependencyResolver resolver)
        {
            INavigationRegistry navigationRegistry = resolver.Resolve<INavigationRegistry>();
            TickerManager tickerManager = resolver.Resolve<TickerManager>();
            LoaderPanelHelper loaderPanelHelper = resolver.Resolve<LoaderPanelHelper>();

            MiniDiContainer localContainer = new MiniDiContainer(resolver);
            ProxyNavigationRegistry proxyNavigationRegistry = new ProxyNavigationRegistry(_navigationName);
            localContainer.BindInterfaceToInstance<INavigationRegistry, ProxyNavigationRegistry>(proxyNavigationRegistry);

            IConfiguratorLoader loader = new AddressableScriptableConfiguratorLoader(_configurator.RuntimeKey);
            IConfiguratorLoader[] loaders = { loader };
            ConfiguratorSet configuratorSet = new ConfiguratorSet(loaders, localContainer, tickerManager);

            ProxyNavigationFactory proxyFactory = new ProxyNavigationFactory(proxyNavigationRegistry, configuratorSet, loaderPanelHelper);

            DisposeOnUnload(navigationRegistry.Register(_navigationName, proxyFactory));
        }
    }
}