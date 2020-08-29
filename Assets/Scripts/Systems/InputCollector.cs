using System;
using System.Collections.Generic;
using Roomba.Systems.Input;
using UnityEngine;
using Zenject;

namespace Roomba.Systems
{
    public class InputCollector : ITickable
    {
        public enum Actions
        {
            Null = 0,
            Accept = 1,
            Interact = 2,
            Cancel = 3,
            Up = 4,
            Down = 5,
            Left = 6,
            Right = 7
        }

        [Serializable]
        public class ButtonAction
        {
            public Actions action;
            public KeyCode button;
        }

        [Serializable]
        public class InputSetting
        {
            public bool useMouse;

            public string[] axis;

            public ButtonAction[] buttons;
            
            [Space]
            public LayerMask mouseMask;
        }

        private readonly SignalBus _signalBus;
        private InputSetting _settings;

        private AxisSignal[] _axis;
        private ActionSignal[] _actions;

        public InputCollector(SignalBus signalBus, InputSetting settings)
        {
            _signalBus = signalBus;
            _settings = settings;

            _axis = new AxisSignal[_settings.axis.Length];

            for (int i = 0; i < _axis.Length; i++)
            {
                _axis[i] = new AxisSignal(i, 0);
            }

            _actions = new ActionSignal[_settings.buttons.Length];

            for (int i = 0; i < _actions.Length; i++)
            {
                _actions[i] = new ActionSignal(_settings.buttons[i].action, false);
            }
        }

        private void AxisListening()
        {
            for (int i = 0; i < _axis.Length; i++)
            {
                if (i <= 1 && _settings.useMouse)
                {
                    if (i == 0)
                        _axis[i].Value = UnityEngine.Input.mousePosition.x;
                    else
                        _axis[i].Value = UnityEngine.Input.mousePosition.y;
                    
                    if (_axis[i].Dirty)
                        _signalBus.Fire(_axis[i]);
                    
                    continue;
                }

                _axis[i].Value = UnityEngine.Input.GetAxisRaw(_settings.axis[i]);

                if (_axis[i].Dirty)
                    _signalBus.Fire(_axis[i]);
            }
        }

        private void KeyListening()
        {
            for (int i = 0; i < _settings.buttons.Length; i++)
            {
                if (UnityEngine.Input.GetKeyDown(_settings.buttons[i].button) && !_actions[i].down)
                {
                    _actions[i].down = true;
                    _signalBus.Fire(_actions[i]);
                }
                else if (UnityEngine.Input.GetKeyUp(_settings.buttons[i].button) && _actions[i].down)
                {
                    _actions[i].down = false;
                    _signalBus.Fire(_actions[i]);
                }
            }
        }

        public void Tick()
        {
            AxisListening();
            KeyListening();
        }
    }
}