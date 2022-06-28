using Application.Root;
using Behc.Configuration;
using Behc.Mvp.Models;
using Behc.Mvp.Presenters;
using Behc.Mvp.Presenters.Factories;
using Behc.Navigation;
using Features.CoreGameplay.Models;
using Features.CoreGameplay.Presenters;
using UnityEngine;

namespace Features.CoreGameplay
{
    [CreateAssetMenu(fileName = "CoreGameplayConfigurator", menuName = "Game/Configurator/CoreGameplay", order = 0)]
    public class CoreGameplayConfigurator : ScriptableConfigurator
    {
#pragma warning disable CS0649
        [SerializeField] private CoreGameplayPanelPresenter _coreGameplayPanelPresenter;
        [SerializeField] private PreparePanelPresenter _preparePanelPresenter;
        [SerializeField] private ResultsPanelPresenter _resultsPanelPresenter;
#pragma warning restore CS0649

        protected override void OnLoad(IDependencyResolver resolver)
        {
            PresenterUpdateKernel kernel = resolver.Resolve<PresenterUpdateKernel>();
            PresenterMap globalPresenterMap = resolver.Resolve<PresenterMap>();
            INavigationRegistry navigationRegistry = resolver.Resolve<INavigationRegistry>();
            NavigationManager navigationManager = resolver.Resolve<NavigationManager>();
         
            DataSlot mainDisplaySlot = resolver.Resolve<DataSlot>(RootElements.MAIN_DISPLAY_SLOT);

            DisposeOnUnload(globalPresenterMap.RegisterFactory<CoreGameplayPanel>(_coreGameplayPanelPresenter.gameObject, kernel));
            DisposeOnUnload(globalPresenterMap.RegisterFactory<PreparePanel>(_preparePanelPresenter.gameObject, kernel));
            DisposeOnUnload(globalPresenterMap.RegisterFactory<ResultsPanel>(_resultsPanelPresenter.gameObject, kernel));

            CoreGameplayPanelFactory gameplayPanelFactory = new CoreGameplayPanelFactory();
            PreparePanelFactory preparePanelFactory = new PreparePanelFactory();
            ResultsPanelFactory resultsPanelFactory = new ResultsPanelFactory();
            CoreGameplayFactory coreGameplayFactory = new CoreGameplayFactory(mainDisplaySlot, gameplayPanelFactory, preparePanelFactory, resultsPanelFactory, navigationManager);

            DisposeOnUnload(navigationRegistry.Register(CoreGameplay.Models.CoreGameplay.NAME, coreGameplayFactory));
        }
    }
}