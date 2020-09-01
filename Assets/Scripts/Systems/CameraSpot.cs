using System;
using UnityEngine;

namespace Roomba.Systems
{
    public class CameraSpot : MonoBehaviour
    {
        public Transform point;
        public BoxCollider area;
    
        private Bounds _availableArea;
        public Vector3 position => point.position;
        public Quaternion rotation => point.rotation;
        public Vector3 forward => point.forward;

        private void Awake()
        {
            _availableArea = new Bounds(area.bounds.center, area.bounds.size);
            area.enabled = false;
        }

        public bool InBounds(Vector3 position)
        {
            return _availableArea.Contains(position);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_availableArea.center, _availableArea.size);
        }
    }
}