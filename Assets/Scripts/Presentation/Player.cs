using System;
using Roomba.Systems;
using Roomba.Systems.Actors;
using Roomba.Systems.Input;
using Roomba.Systems.Interfaces;
using UnityEngine;
using Zenject;

namespace Roomba.Presentation
{
    public class Player : Actor
    {
        [Inject] private SignalBus _signal;
        [Inject] private InputCollector.InputSetting _setting;
        private Ray _ray;
        private Vector3 _axis;
        private IInteractable _interactable;

        protected override void Awake()
        {
            base.Awake();
            
            _signal.Subscribe<AxisSignal>(AxisInput);
            _signal.Subscribe<ActionSignal>(ActionInput);
        }

        private void ActionInput(ActionSignal action)
        {
            switch (action.actions)
            {
                case InputCollector.Actions.Interact:
                    Interact();
                    break;
            }
        }

        private void Interact()
        {
            _interactable?.Interact();
        }

        void SetupMouseLook()
        {
            if (!_setting.useMouse) return;
            
            lookDirection.x = _axis.x;
            lookDirection.y = _axis.y;
            
            lookDirection.z = camera.camera.nearClipPlane;
                
            _ray = camera.camera.ScreenPointToRay(lookDirection);
                
            if (Physics.Raycast(_ray, out var hit, 1000, _setting.mouseMask))
            {
                lookDirection = hit.point;
                Debug.DrawRay(_ray.origin, _ray.direction * hit.distance, Color.red);
            }
            else
            {
                lookDirection.z = (transform.position - camera.transform.position).magnitude;

                Debug.DrawRay(_ray.origin, _ray.direction * 1000, Color.green);
                lookDirection = camera.camera.ScreenToWorldPoint(lookDirection);
            }
                
            lookDirection = lookDirection - transform.position;
            lookDirection.y = 0;

            lookDirection.Normalize();
        }

        protected override void FixedUpdate()
        {
            SetupMouseLook();
            
            base.FixedUpdate();
        }

        private void AxisInput(AxisSignal axis)
        {
            switch (axis.id)
            {
                case 0:
                    _axis.x = axis.Value;
                    break;
                case 1:
                    _axis.y = axis.Value;
                    break;
            }
            
            lookDirection.x = _axis.x;
            lookDirection.y = _axis.y;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            
            _interactable = other.GetComponent<IInteractable>();
        }

        protected override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);
            
            if (_interactable == null) return;

            var reference = other.GetComponent<IInteractable>();
            
            if (reference == _interactable)
                _interactable = null;
        }
    }
}