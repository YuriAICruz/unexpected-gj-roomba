using System;
using Roomba.Systems.Interfaces;
using UnityEngine;
using Zenject;

namespace Roomba.Systems.Actors
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Actor : MonoBehaviour,IDamageable
    {
        public int Hp => _hp;

        public float speed = 5;
        public float turnSpeed = 2;
        
        [SerializeField]
        private int _hp = 5;
        
        private Vector3 _velocity;
        private Rigidbody _rigidbody;

        [Inject] protected CameraController camera;
        
        protected Vector3 lookDirection;
        
        protected Vector3 direction;


        protected virtual void Awake()
        {
            direction = transform.forward;
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        protected virtual void Update()
        {
            
        }
        
        protected virtual void FixedUpdate()
        {
            Move();
        }

        protected void Move()
        {
            direction = Vector3.RotateTowards(
                direction,
                lookDirection,
                turnSpeed,
                1
            );
            
            _velocity = speed * direction; //_camera.transform.TransformDirection(_input);

            _velocity.y = _rigidbody.velocity.y;

            _rigidbody.velocity = _velocity;

            transform.forward = direction;
        }
    }
}