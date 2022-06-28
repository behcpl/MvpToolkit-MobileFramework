using Behc.Mvp.Presenters;
using Features.MainPanelWithBottomBar.Models;
using UnityEngine;

namespace Features.MainPanelWithBottomBar.Presenters
{
    public class MainPanelPresenter : PanelBase<MainPanel>
    {
#pragma warning disable CS0649
        [SerializeField] private DataSlotPresenter _contentPresenter;
        [SerializeField] private BottomBarPresenter _bottomBarPresenter;
#pragma warning restore CS0649
        
        protected override void OnInitialize()
        {
            RegisterPresenter(m => m.Content, _contentPresenter);
            RegisterPresenter(m => m.BottomBar, _bottomBarPresenter);
        }
    }
}