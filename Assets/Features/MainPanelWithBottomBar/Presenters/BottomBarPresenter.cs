using Behc.Mvp.Presenters;
using Behc.Mvp.Utils;
using Features.MainPanelWithBottomBar.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Features.MainPanelWithBottomBar.Presenters
{
    public class BottomBarPresenter : PanelBase<BottomBar>
    {
#pragma warning disable CS0649
        [SerializeField] private Button[] _buttons;
        [SerializeField] private Image[] _backgrounds;
#pragma warning restore CS0649

        protected override void OnBind(bool prepareForAnimation)
        {
            Debug.Assert(_buttons.Length == BottomBar.BUTTONS_COUNT, "_buttons.Length == BottomBar.BUTTONS_COUNT (5)");
            Debug.Assert(_backgrounds.Length == BottomBar.BUTTONS_COUNT, "_backgrounds.Length == BottomBar.BUTTONS_COUNT (5)");
            
            DisposeOnUnbind(_model.Subscribe(Refresh));
            
            Refresh();

            for (int i = 0; i < BottomBar.BUTTONS_COUNT; i++)
            {
                BottomBar.ButtonType button = (BottomBar.ButtonType)i;
                DisposeOnUnbind(_buttons[i].onClick.Subscribe(() => _model.ButtonClicked(button)));
            }
        }

        private void Refresh()
        {
            for (int i = 0; i < BottomBar.BUTTONS_COUNT; i++)
            {
                _buttons[i].interactable = _model.GetStatus((BottomBar.ButtonType)i) != BottomBar.ButtonStatus.LOCKED;
                _backgrounds[i].color = _model.GetStatus((BottomBar.ButtonType)i) == BottomBar.ButtonStatus.SELECTED ? Color.red : Color.gray;
            }
        }
    }
}