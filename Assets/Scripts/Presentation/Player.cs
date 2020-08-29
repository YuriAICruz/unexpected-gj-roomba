using Roomba.Systems;
using Roomba.Systems.Actors;
using Roomba.Systems.Input;
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

        protected override void Awake()
        {
            base.Awake();
            
            _signal.Subscribe<AxisSignal>(AxisInput);
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

            if (_setting.useMouse)
            {
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
        }
    }
}