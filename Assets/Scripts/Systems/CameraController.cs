using System;
using UnityEngine;

namespace Roomba.Systems
{
    public class CameraController : MonoBehaviour
    {
        [HideInInspector]
        public Camera camera;

        private void Awake()
        {
            camera = GetComponent<Camera>();
        }
    }
}