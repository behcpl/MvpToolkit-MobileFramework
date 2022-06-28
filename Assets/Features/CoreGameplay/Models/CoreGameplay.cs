using Behc.Mvp.Models;
using Behc.Navigation;
using Behc.Utils;
using UnityEngine;

namespace Features.CoreGameplay.Models
{
    public class CoreGameplay : NavigableBase
    {
        public const string NAME = nameof(CoreGameplay);

        private readonly DataSlot _fullscreenPanels;
        private readonly IFactory<CoreGameplayPanel> _mainPanelFactory;
        private readonly IFactory<CoreGameplay, PreparePanel> _preparePanelFactory;
        private readonly IFactory<ResultsPanel> _resultsPanelFactory;
        private readonly NavigationManager _navigationManager;
        
        public CoreGameplay(
            DataSlot fullscreenPanels, 
            IFactory<CoreGameplayPanel> mainPanelFactory,
            IFactory<CoreGameplay, PreparePanel> preparePanelFactory,
            IFactory<ResultsPanel> resultsPanelFactory, 
            NavigationManager navigationManager)
        {
            _fullscreenPanels = fullscreenPanels;
            _mainPanelFactory = mainPanelFactory;
            _preparePanelFactory = preparePanelFactory;
            _resultsPanelFactory = resultsPanelFactory;
            _navigationManager = navigationManager;
        }

        protected override void OnStart()
        {
            PreparePanel panel = _preparePanelFactory.Create(this);
            _fullscreenPanels.Data = panel;
        }

        public void Close()
        {
            bool did = _navigationManager.NavigateBack();
            Debug.Log($"CoreGameplay::Close {did}");
        }
    }
    
    public class CoreGameplayFactory : IFactory<object, INavigable>
    {
        private readonly DataSlot _fullscreenPanels;
        private readonly IFactory<CoreGameplayPanel> _mainPanelFactory;
        private readonly IFactory<CoreGameplay, PreparePanel> _preparePanelFactory;
        private readonly IFactory<ResultsPanel> _resultsPanelFactory;
        private readonly NavigationManager _navigationManager;

        public CoreGameplayFactory(
            DataSlot fullscreenPanels, 
            IFactory<CoreGameplayPanel> mainPanelFactory, 
            IFactory<CoreGameplay, PreparePanel> preparePanelFactory, 
            IFactory<ResultsPanel> resultsPanelFactory,
            NavigationManager navigationManager)
        {
            _fullscreenPanels = fullscreenPanels;
            _mainPanelFactory = mainPanelFactory;
            _preparePanelFactory = preparePanelFactory;
            _resultsPanelFactory = resultsPanelFactory;
            _navigationManager = navigationManager;
        }

        public INavigable Create(object parameters)
        {
            return new CoreGameplay(_fullscreenPanels, _mainPanelFactory, _preparePanelFactory, _resultsPanelFactory, _navigationManager);
        }
    }
}