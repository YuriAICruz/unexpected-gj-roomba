using System;
using UnityEngine;
using Zenject;

namespace Roomba.Systems
{
    public class CameraController : MonoBehaviour
    {
        public CameraSpot[] spots;

        public float speed;

        [Inject] private GameManager _gameManager;

        [HideInInspector] public Camera camera;

        private int _currentSpot;
        private Vector3 _center;

        private void Awake()
        {
            camera = GetComponent<Camera>();

            transform.SetPositionAndRotation(
                spots[_currentSpot].position,
                spots[_currentSpot].rotation
            );
        }

        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, spots[_currentSpot].position,
                Time.deltaTime * speed);
            transform.forward = Vector3.RotateTowards(transform.forward,
                spots[_currentSpot].forward, Time.deltaTime * speed, 1);
        }

        private void FixedUpdate()
        {
            CalculateCenter();
            
            if (spots[_currentSpot].InBounds(_center))
                return;

            var pos = _center;
            var i = 0;
            var success = false;
            for (i = 0; i < spots.Length; i++)
            {
                if (spots[i].InBounds(pos))
                {
                    success = true;
                    break;
                }
            }

            if (success)
                _currentSpot = i;
        }

        private void CalculateCenter()
        {
            _center = _gameManager.Center;
        }
    }
}