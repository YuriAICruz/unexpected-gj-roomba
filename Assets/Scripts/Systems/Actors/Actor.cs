using System;
using Mirror;
using Roomba.Systems.Interfaces;
using UnityEngine;
using Zenject;

namespace Roomba.Systems.Actors
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Actor : NetworkBehaviour, IDamageable
    {
        public int Hp => _hp;

        public float speed = 6;
        public float turnSpeed = 0.2f;
        public float maxSlope = 28;

        [SerializeField] private int _hp = 5;

        private Vector3 _velocity;
        private Rigidbody _rigidbody;

        [Inject] protected CameraController camera;

        protected Vector3 lookDirection;

        protected Vector3 direction;
        protected Vector3 groundNormal;
        protected bool grounded;

        [SyncVar] private Vector3 _relativePosition;
        [SyncVar] private Vector3 _relativeLook;

        private Vector3 _spawnPoint;


        protected virtual void Awake()
        {
            direction = transform.forward;
            _rigidbody = GetComponent<Rigidbody>();
            
        }

        protected virtual void Start()
        {
            _spawnPoint = transform.position;

            if (!isLocalPlayer)
            {
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.isKinematic = true;
            }
        }

        private void Update()
        {
            if (isLocalPlayer)
            {
                LocalUpdate();
            }
            else
            {
                ClientsUpdate();
            }
        }

        protected virtual void LocalUpdate()
        {
        }

        protected virtual void ClientsUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, _relativePosition, Time.deltaTime * 5f);
            transform.forward = Vector3.RotateTowards(transform.forward, _relativeLook, Time.deltaTime * 5f, 1);
        }

        protected virtual void FixedUpdate()
        {
            if (isLocalPlayer)
            {
                Move();

                var pos = transform.position;
                SetPosition(pos, transform.forward);

                if (pos.y <= -5)
                    Respawn();
            }
        }

        private void Respawn()
        {
            transform.position = _spawnPoint;
            _rigidbody.velocity = _velocity = Vector3.zero;
        }

        [Command]
        protected void SetPosition(Vector3 position, Vector3 forward)
        {
            _relativePosition = position;
            _relativeLook = forward;
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

        #region Physics

        protected virtual void OnTriggerEnter(Collider other)
        {
        }

        protected virtual void OnTriggerStay(Collider other)
        {
        }

        protected virtual void OnTriggerExit(Collider other)
        {
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

        protected virtual void OnCollisionStay(Collision other)
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

        protected virtual void OnCollisionExit(Collision other)
        {
        }

        #endregion
    }
}