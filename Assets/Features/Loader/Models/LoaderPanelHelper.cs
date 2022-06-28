using System;
using Behc.Mvp.Models;
using Behc.Utils;

namespace Features.Loader.Models
{
    public class LoaderPanelHelper
    {
        private readonly DataSlot _loaderParentSlot;
        private readonly IFactory<Action<LoaderPanel>, Action<LoaderPanel>, Action<LoaderPanel>, LoaderPanel> _loaderPanelFactory;

        public LoaderPanelHelper(
            DataSlot loaderParentSlot,
            IFactory<Action<LoaderPanel>, Action<LoaderPanel>, Action<LoaderPanel>, LoaderPanel> loaderPanelFactory)
        {
            _loaderParentSlot = loaderParentSlot;
            _loaderPanelFactory = loaderPanelFactory;
        }

        public void Setup(string message, Action<LoaderPanel> onReady, Action<LoaderPanel> onPoll, Action<LoaderPanel> onElapsedMinimalTime)
        {
            LoaderPanel panel = _loaderPanelFactory.Create(onReady, onPoll, onElapsedMinimalTime);
            panel.Advance(message, 0);
            _loaderParentSlot.Data = panel;
        }
    }
}