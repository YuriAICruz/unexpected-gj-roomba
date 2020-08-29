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
        public float maxSlope = 32;
        
        [SerializeField]
        private int _hp = 5;
        
        private Vector3 _velocity;
        private Rigidbody _rigidbody;

        [Inject] protected CameraController camera;
        
        protected Vector3 lookDirection;
        
        protected Vector3 direction;
        protected Vector3 groundNormal;
        protected bool grounded;


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

        protected virtual void OnCollisionEnter(Collision other)
        {
            var normal = Vector3.zero;

            for (int i = 0; i < other.contacts.Length; i++)
            {
                normal += other.contacts[i].normal;
            }

            if (other.transform.CompareTag("DoNotBounce"))
            {
                return;
            }

            normal /= other.contacts.Length;

            var angle = Vector3.Angle(normal, Vector3.up);

            if (angle > 80) // wall
            {
                direction = normal;
                direction.y = 0;
            }
            else // ground
            {
                if (other.contacts[0].point.y > transform.position.y)
                {
                    Debug.Log("----");
                    var pos = transform.position;
                    pos.y = other.contacts[0].point.y;
                    transform.position = pos;
                }
            }
        }

        protected virtual  void OnCollisionStay(Collision other)
        {
            var normal = Vector3.zero;

            for (int i = 0; i < other.contacts.Length; i++)
            {
                normal += other.contacts[i].normal;
            }

            normal /= other.contacts.Length;

            var angle = Vector3.Angle(normal, Vector3.up);

            if (angle > 80) // wall
            {
                return;
            }

            if (angle > maxSlope) // ground
            {
                groundNormal = normal;
                grounded = true;
            }
            else
            {
                groundNormal = normal;
                grounded = false;
            }
        }

        protected virtual  void OnCollisionExit(Collision other)
        {
        }
    }
}