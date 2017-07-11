using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

namespace NostrumGames
{
    public class PlayerController : MonoBehaviour
    {
        public IObservable<Unit> MovingVelocity { get; private set; }
        public static float GravityScaleDefualt { get; private set; }
        public float VelocityX { get { return _velocityX; } private set { } }
        private Rigidbody2D _rigidbody2D;
        [SerializeField]
        private float _upForce;
        [SerializeField]
        private float _gravityScale;
        [SerializeField]
        private float _velocityX;
        [SerializeField]
        [Range(0, 1)]
        private float _upwardDrag;

        private IDisposable _moveUp;
        private IDisposable _movingOnX;

        void Awake()
        {
            _rigidbody2D = this.GetComponent<Rigidbody2D>();
            GravityScaleDefualt = _rigidbody2D.gravityScale;
        }

        void Start()
        {
            InitBasicMovement();



        }
        private void InitBasicMovement()
        {
            _moveUp = MyInputs.Instance.MoveUp
            .Subscribe(pressingSpace =>
            {
                if (pressingSpace)
                {
                    _rigidbody2D.AddForce(Vector2.up * (_upForce * _rigidbody2D.mass));
                }
                else
                {
                    if (_rigidbody2D.velocity.y > 0)
                    {
                        var vel = _rigidbody2D.velocity;
                        vel.y *= _upwardDrag;
                        _rigidbody2D.velocity = vel;
                    }
                }
            })
            .AddTo(this);

            MovingOnAxisX();
        }

        private void MovingOnAxisX()
        {
            _movingOnX = this.FixedUpdateAsObservable()
                .Subscribe(_ => _rigidbody2D.velocity = new Vector2(_velocityX, _rigidbody2D.velocity.y))
                .AddTo(this);
        }

        public void StartNewLife()
        {
            _rigidbody2D.gravityScale = GravityScaleDefualt;
            _rigidbody2D.isKinematic = false;
            _rigidbody2D.velocity = new Vector2(0, 0);
            InitBasicMovement();
        }

        //TODO: make better "death gravity", now you have to set the gravityScale back to default
        public void KillController()
        {
            _moveUp.Dispose();
            _movingOnX.Dispose();
            _rigidbody2D.velocity = Vector3.zero;
            _rigidbody2D.gravityScale = _gravityScale;
            // rigidbody2D.isKinematic = true;
        }

        public void IsKinematic(bool kinematic)
        {
            _rigidbody2D.isKinematic = kinematic;
        }

    }
}
