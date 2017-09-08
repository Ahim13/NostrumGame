using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

namespace NostrumGames
{

    public enum ControllerType
    {
        Basic,
        Reflected,

    }

    public class PlayerController : PlayerBase
    {
        public IObservable<Unit> MovingVelocity { get; private set; }
        public static float GravityScaleDefualt { get; private set; }
        public float VelocityX { get { return _velocityX; } private set { } }

        private Rigidbody2D _rigidbody2D;
        private BoxCollider2D _boxCollider2D;
        private ControllerType _controllerType;

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
            _boxCollider2D = this.GetComponent<BoxCollider2D>();
            GravityScaleDefualt = _rigidbody2D.gravityScale;
            _controllerType = ControllerType.Basic;
        }

        void Start()
        {
            InitBasicMovement();

        }
        private void InitBasicMovement()
        {
            _moveUp = MyInputs.Instance.MoveUp
                // .Where(_ => PhotonViewManagerOnPlayer.IsPhotonViewMine())
                .Subscribe(pressingSpace =>
                {
                    MovementBasenOnControllType(pressingSpace);
                })
                .AddTo(this);

            MovingOnAxisX();
        }

        private void MovementBasenOnControllType(bool pressingSpace)
        {
            switch (_controllerType)
            {
                case ControllerType.Basic:
                    MovementMechanic(pressingSpace, Vector2.up, 1);
                    break;
                case ControllerType.Reflected:
                    MovementMechanic(pressingSpace, Vector2.down, -1);
                    break;
                default:
                    MovementMechanic(pressingSpace, Vector2.up, 1);
                    break;
            }

        }

        private void MovementMechanic(bool pressingSpace, Vector2 direction, int signum)
        {
            if (pressingSpace)
            {
                _rigidbody2D.AddForce(direction * (_upForce * _rigidbody2D.mass));
            }
            else if (signum * _rigidbody2D.velocity.y > 0)
            {
                var vel = _rigidbody2D.velocity;
                vel.y *= _upwardDrag;
                _rigidbody2D.velocity = vel;
            }
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
            _controllerType = ControllerType.Basic;

            IsBoxCollider2DEnabled(true);
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

        public void IsBoxCollider2DEnabled(bool enabled)
        {
            _boxCollider2D.enabled = enabled;
        }

        public void ChangeControllerTypeAndGravity(ControllerType newControllType)
        {
            _controllerType = newControllType;

            switch (_controllerType)
            {
                case ControllerType.Basic:
                    _rigidbody2D.gravityScale = GravityScaleDefualt;
                    break;
                case ControllerType.Reflected:
                    _rigidbody2D.gravityScale = GravityScaleDefualt * -1;
                    break;
                default:
                    _rigidbody2D.gravityScale = GravityScaleDefualt;
                    break;
            }
        }
    }
}
