using System;
using Behc.Configuration;
using Behc.Configuration.Loaders;
using Behc.Mvp.Models;
using Behc.Mvp.Presenters;
using Behc.Navigation;
using Behc.Utils;
using Features.Loader.Models;
using Features.MainPanelWithBottomBar.Models;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Application.Root
{
    public class Root : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField] private PresenterUpdateKernel _updateKernel;
        [SerializeField] private RectTransform _defaultContainer;
        [SerializeField] private RectTransform _defaultPool;

        [SerializeField] private DataSlotPresenter _mainDisplaySlotPresenter;
        [SerializeField] private DataStackPresenter _windowStackPresenter;
        [SerializeField] private ToolTipManagerPresenter _toolTipManagerPresenter;
        [SerializeField] private ToastManagerPresenter _toastManagerPresenter;

        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private Camera _mainCamera;
    
        [SerializeField] private ScriptableConfigurator[] _preloadConfigurators;
#pragma warning restore CS0649

        private MiniDiContainer _diContainer;
        private TickerManager _tickerManager;
        private BackButtonManager _backButtonManager;
        private NavigationManager _navigationManager;

        private bool _deselect;

        private void Start()
        {
            Screen.SetResolution(1080, 1920, FullScreenMode.Windowed);

            _diContainer = new MiniDiContainer();
            
            _tickerManager = new TickerManager();
            _diContainer.BindInstance(_tickerManager);
           
            RegisterNavigation();
            RegisterPresenterElements();
            RegisterRootDataModels(out DataSlot mainDisplaySlot);
            RegisterLoaderPanel(mainDisplaySlot, out LoaderPanelHelper loaderPanelHelper);
            RegisterServices();
            
            PreloadConfigurators();  

            SetupBackButtonActions();
            WireUpBlockingLayers();
    
            IConfiguratorLoader[] loaders = { new AddressableScriptableConfiguratorLoader("DefaultConfigurator") };
            ConfiguratorSet configuratorSet = new ConfiguratorSet(loaders, _diContainer, _tickerManager);
            ApplicationMain applicationMain = new ApplicationMain(_navigationManager, loaderPanelHelper, configuratorSet);
            applicationMain.Start();
     
            //initialize after ApplicationMain.Start, so loader is already set, and will not trigger any transition animation 
            _updateKernel.InitializePresenters(InitializePresenters);
        }
      
        private void RegisterNavigation()
        {
            _backButtonManager = new BackButtonManager();
            _diContainer.BindInstance(_backButtonManager);

            NavigationRegistry navigationRegistry = new NavigationRegistry();
            _diContainer.BindInterfaceToInstance<INavigationRegistry, NavigationRegistry>(navigationRegistry);

            _navigationManager = new NavigationManager(navigationRegistry);
            _diContainer.BindInstance(_navigationManager);
        }

        private void RegisterPresenterElements()
        {
            _diContainer.BindInstance(_updateKernel);
            _diContainer.BindNamedInstance(RootElements.DEFAULT_CONTAINER, _defaultContainer);
            _diContainer.BindNamedInstance(RootElements.DEFAULT_POOL, _defaultPool);
            _diContainer.BindInstance(new PresenterMap(null));
            _diContainer.BindInstance(_mainCamera);
            
            _diContainer.BindNamedInstance(RootElements.MAIN_DISPLAY_SLOT_PRESENTER,_mainDisplaySlotPresenter);
            _diContainer.BindInstance(_windowStackPresenter);
            _diContainer.BindInstance(_toolTipManagerPresenter);
            _diContainer.BindInstance(_toastManagerPresenter);
        }

        private void RegisterRootDataModels(out DataSlot mainDisplaySlot)
        {
            mainDisplaySlot = new DataSlot();
           
            _diContainer.BindNamedInstance(RootElements.MAIN_DISPLAY_SLOT, mainDisplaySlot);
            _diContainer.BindInstance(new DataStack());
            _diContainer.BindInstance(new ToolTipManager());
            _diContainer.BindInstance(new ToastManager());
            
            _diContainer.BindInstance(new MainPanel(_navigationManager));
        }

        private void RegisterLoaderPanel(DataSlot loaderParentSlot, out LoaderPanelHelper loaderPanelHelper)
        {
            IFactory<Action<LoaderPanel>, Action<LoaderPanel>, Action<LoaderPanel>, LoaderPanel> factory = new LoaderPanelFactory();
            _diContainer.BindInstance(factory);

            loaderPanelHelper = new LoaderPanelHelper(loaderParentSlot, factory);
            _diContainer.BindInstance(loaderPanelHelper);
        }

        private void RegisterServices()
        {
        //     ExampleServiceMock exampleService = new ExampleServiceMock();
        //     _diContainer.BindInterfaceToInstance<IExampleService, ExampleServiceMock>(exampleService);
        }

        private void PreloadConfigurators()
        {
            if (_preloadConfigurators == null)
                return;

            foreach (IConfigurator configurator in _preloadConfigurators)
            {
                configurator.Load(_diContainer);
            }
        }

        private void InitializePresenters(PresenterUpdateKernel kernel)
        {
            PresenterMap globalMap = _diContainer.Resolve<PresenterMap>();

            _mainDisplaySlotPresenter.Initialize(globalMap, kernel);
            _mainDisplaySlotPresenter.Bind(_diContainer.Resolve<DataSlot>(RootElements.MAIN_DISPLAY_SLOT), null, false);
            _mainDisplaySlotPresenter.Activate();

            _windowStackPresenter.Initialize(globalMap, kernel);
            _windowStackPresenter.Bind(_diContainer.Resolve<DataStack>(), null, false);
            _windowStackPresenter.Activate();

            _toolTipManagerPresenter.Initialize(globalMap, kernel);
            _toolTipManagerPresenter.Bind(_diContainer.Resolve<ToolTipManager>(), null, false);

            _toastManagerPresenter.Initialize(globalMap, kernel);
            _toastManagerPresenter.Bind(_diContainer.Resolve<ToastManager>(), null, false);
        }

        private void SetupBackButtonActions()
        {
            DataStack windowStack = _diContainer.Resolve<DataStack>();
            
            _backButtonManager.RegisterHandler(CancelSelection, 1000).KeepForever();
            _backButtonManager.RegisterHandler(windowStack.TryRemoveTopLevel, 800).KeepForever();
        }

        private void WireUpBlockingLayers()
        {
            _windowStackPresenter.OnBlockingStatusChange += _mainDisplaySlotPresenter.SetBlockedStatus;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _backButtonManager.BackButtonClicked();
            }

            if (_deselect)
            {
                _eventSystem.SetSelectedGameObject(null);
                _deselect = false;
            }

            _navigationManager.CommitDeferredChanges();
            
            _tickerManager.Update();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            //TODO: schedule system notifications if if paused
            //TODO: restore network connection if resumed
        }

        private bool CancelSelection()
        {
            if (_eventSystem.currentSelectedGameObject == null)
                return false;

            _deselect = true;
            return true;
        }
    }
}