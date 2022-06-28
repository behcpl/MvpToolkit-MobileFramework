using System;
using Behc.Mvp.Models;
using Behc.Utils;
using UnityEngine;

namespace Features.MainPanelWithBottomBar.Models
{
    public class BottomBar : ReactiveModel
    {
        public const int BUTTONS_COUNT = 5;
        
        public enum ButtonStatus
        {
            LOCKED,
            NORMAL,
            SELECTED,
        }

        public enum ButtonType
        {
            START,
            UNITS,
            CAMPAIGN,
            LOCKED_EXAMPLE,
            SOCIAL
        }

        public Action<ButtonType> OnButtonClicked;
        
        private readonly ButtonStatus[] _buttons;
        
        public BottomBar()
        {
            _buttons = new ButtonStatus[BUTTONS_COUNT];
            _buttons[(int)ButtonType.START] = ButtonStatus.SELECTED;
            _buttons[(int)ButtonType.UNITS] = ButtonStatus.NORMAL;
            _buttons[(int)ButtonType.CAMPAIGN] = ButtonStatus.NORMAL;
            _buttons[(int)ButtonType.LOCKED_EXAMPLE] = ButtonStatus.LOCKED;
            _buttons[(int)ButtonType.SOCIAL] = ButtonStatus.NORMAL;
        }

        public ButtonStatus GetStatus(ButtonType button)
        {
            return _buttons[(int)button];
        }

        public void Lock(ButtonType button)
        {
            int index = (int)button;
            if (_buttons[index] != ButtonStatus.NORMAL)
                return;
            
            _buttons[index] = ButtonStatus.LOCKED;
            NotifyChanges();           
        }
        
        public void Unlock(ButtonType button)
        {
            int index = (int)button;
            if (_buttons[index] != ButtonStatus.LOCKED)
                return;
            
            _buttons[index] = ButtonStatus.NORMAL;
            NotifyChanges();
        }
        
        public void Select(ButtonType button)
        {
            int index = (int)button;
            if (_buttons[index] != ButtonStatus.NORMAL)
                return;

            for (int i = 0; i < _buttons.Length; i++)
            {
                if (_buttons[i] == ButtonStatus.SELECTED)
                    _buttons[i] = ButtonStatus.NORMAL;
            }
            
            _buttons[index] = ButtonStatus.SELECTED;
            NotifyChanges(); 
        }

        public void ButtonClicked(ButtonType button)
        {
            Debug.Log($"ButtonClicked: {button}");
            OnButtonClicked?.Invoke(button);
        }
    }

    public class BottomBarFactory : IFactory<BottomBar>
    {
        public BottomBar Create()
        {
            return new BottomBar();
        }
    }
}