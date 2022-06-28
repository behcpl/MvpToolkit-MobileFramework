using Behc.Configuration;
using Behc.Mvp.Presenters;
using Behc.Mvp.Presenters.Factories;
using Features.MainPanelWithBottomBar.Models;
using Features.MainPanelWithBottomBar.Presenters;
using UnityEngine;

namespace Features.DefaultConfiguration
{
    [CreateAssetMenu(fileName = "DefaultConfigurator", menuName = "Game/Configurator/Default", order = 0)]
    public class DefaultConfigurator : ScriptableConfigurator
    {
#pragma warning disable CS0649
        [SerializeField] private MainPanelPresenter _mainPanelPresenter;
#pragma warning restore CS0649

        protected override void OnLoad(IDependencyResolver resolver)
        {
            PresenterUpdateKernel kernel = resolver.Resolve<PresenterUpdateKernel>();
            PresenterMap globalPresenterMap = resolver.Resolve<PresenterMap>();

            DisposeOnUnload(globalPresenterMap.RegisterFactory<MainPanel>(_mainPanelPresenter.gameObject, kernel));
        }
    }
}