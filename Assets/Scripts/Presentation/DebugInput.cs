using Roomba.Systems;
using Roomba.Systems.Input;
using UnityEngine;
using Zenject;

namespace Roomba.Presentation
{
    public class DebugInput : MonoBehaviour
    {
        [Inject] private InputCollector _input;
        [Inject] private SignalBus _signal;
        
        
        public void Awake()
        {
            _signal.Subscribe<AxisSignal>(OnAxis);
            _signal.Subscribe<ActionSignal>(OnAction);
        }

        private void OnAction(ActionSignal action)
        {
            Debug.Log(action);
        }

        private void OnAxis(AxisSignal axis)
        {
            Debug.Log(axis);
        }
    }
}