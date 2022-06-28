using Behc.Configuration;
using Behc.Mvp.Presenters;
using Behc.Mvp.Presenters.Factories;
using Features.Loader.Models;
using Features.Loader.Presenters;
using UnityEngine;

namespace Features.Loader
{
    [CreateAssetMenu(fileName = "LoaderConfigurator", menuName = "Game/Configurator/LoaderConfigurator", order = 0)]
    public class LoaderConfigurator : ScriptableConfigurator
    {
#pragma warning disable CS0649
        [SerializeField] private LoaderPanelPresenter _loaderPanelPresenter;
#pragma warning restore CS0649
       
        protected override void OnLoad(IDependencyResolver resolver)
        {
            PresenterMap globalMap = resolver.Resolve<PresenterMap>();
            PresenterUpdateKernel kernel = resolver.Resolve<PresenterUpdateKernel>();

            DisposeOnUnload(globalMap.RegisterFactory<LoaderPanel>(_loaderPanelPresenter.gameObject, kernel));
        }
    }
}