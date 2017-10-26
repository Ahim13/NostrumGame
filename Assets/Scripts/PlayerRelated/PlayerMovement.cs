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
        ZeroGravity

    }

    public class NetworkedPlayerMovementInfo
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public double LastNetworkDataReceived { get; set; }
        public float TimeSinceLastUpdate { get { return (float)(PhotonNetwork.time - LastNetworkDataReceived); } }
        public float SpeedOnY { get { return Velocity.y; } }
        public float SpeedOnX { get { return Velocity.x; } }

        public NetworkedPlayerMovementInfo()
        {
            Position = Vector2.zero;
            Velocity = Vector2.zero;
            LastNetworkDataReceived = 0.0;
        }
    }

    public class PlayerMovement : PlayerBase
    {
        public IObservable<Unit> MovingVelocity { get; private set; }
        public ReactiveProperty<float> ReactiveVelocityX { get; private set; }

        private Rigidbody2D _rigidbody2D;
        private BoxCollider2D _boxCollider2D;
        private ControllerType _controllerType;

        [SerializeField]
        private float _upForce = 25f;
        [SerializeField]
        private float _VerticalVelocity = 10f;
        [SerializeField]
        private float _gravityScale = 10f;
        [SerializeField]
        private float _velocityX = 5f;
        [SerializeField]
        [Range(0, 1)]
        private float _upwardDrag = 0.7f;

        private IDisposable _moveUp;
        private IDisposable _movingOnX;

        private NetworkedPlayerMovementInfo _networkedPlayerMovementInfo;
        private NetworkedPlayerMovementInfo _previousInfo;



        public double currentTime = 0.0;
        public double timeToReachGoal = 0.0;


        void Awake()
        {
            InitVairables();

            if (!PhotonViewManagerOnPlayer.IsPhotonViewMine())
            {
                _networkedPlayerMovementInfo = new NetworkedPlayerMovementInfo();
                _previousInfo = new NetworkedPlayerMovementInfo();
                tag = "OtherPlayer";
                return;
            }


            //Init reactiveVeloX - in start it gives nullrefernce issues
            ReactiveVelocityX = this.FixedUpdateAsObservable()
                .Where(_ => PhotonViewManagerOnPlayer.IsPhotonViewMine())
                .Select(_ => _velocityX)
                .Do(velo =>
                {
                    if (Global.PlayersSpeed != velo) Global.PlayersSpeed = velo;
                })
                .ToReactiveProperty()
                .AddTo(this);

        }

        void Start()
        {
            if (!PhotonViewManagerOnPlayer.IsPhotonViewMine()) return;

            InitBasicMovement();

        }

        //If its not our charackter then update its position given by the received NetworkedPlayerMovement values
        void Update()
        {

            if (PhotonViewManagerOnPlayer.IsPhotonViewMine()) return;

            UpdateNetworkedPostion();
        }

        void FixedUpdate()
        {

            //GetComponent<Rigidbody2D>().position = Vector2.Lerp(transform.position, p, Time.deltaTime);
        }

        // private void UpdateNetworkedPostion()
        // {

        //     timeToReachGoal = _networkedPlayerMovementInfo.LastNetworkDataReceived - _previousInfo.LastNetworkDataReceived;
        //     currentTime += Time.deltaTime;


        //     //extrapolate
        //     // Vector2 extrapolatedTargetPosition = new Vector2(
        //     //     (_networkedPlayerMovementInfo.Position.x + _networkedPlayerMovementInfo.SpeedOnX * (float)timeToReachGoal),
        //     //     (_networkedPlayerMovementInfo.Position.y + _networkedPlayerMovementInfo.SpeedOnY * (float)timeToReachGoal)
        //     //     );

        //     // timeToReachGoal = _networkedPlayerMovementInfo.LastNetworkDataReceived - _previousInfo.LastNetworkDataReceived;
        //     // currentTime += Time.deltaTime;
        //     // if (timeToReachGoal != 0) transform.position = Vector2.Lerp(_previousInfo.Position, _networkedPlayerMovementInfo.Position, (float)(currentTime / timeToReachGoal));

        //     if (timeToReachGoal != 0) transform.position = Vector2.Lerp(_previousInfo.Position, _networkedPlayerMovementInfo.Position, (float)(currentTime / timeToReachGoal));


        // }
        //Beginner type of prediction implementation to movement TODO: find a better one for specificly 2D plastformer
        private void UpdateNetworkedPostion()
        {
            float pingInSeconds = (float)PhotonNetwork.GetPing() * 0.001f;
            float timeSinceLastUpdate = (float)(PhotonNetwork.time - _networkedPlayerMovementInfo.LastNetworkDataReceived);
            float totalTimePassed = pingInSeconds + timeSinceLastUpdate;

            var magnitude = _networkedPlayerMovementInfo.Velocity.magnitude;

            Vector2 extrapolatedTargetPosition = new Vector2(
                (_networkedPlayerMovementInfo.Position.x + _networkedPlayerMovementInfo.SpeedOnX * totalTimePassed),
                (_networkedPlayerMovementInfo.Position.y + _networkedPlayerMovementInfo.SpeedOnY * totalTimePassed)
                );

            if (magnitude == 0) magnitude = 25;


            Vector2 newPosition = Vector2.MoveTowards(transform.position, extrapolatedTargetPosition, magnitude * Time.deltaTime);
            // Vector2 newPosition = Vector2.Lerp(transform.position, extrapolatedTargetPosition, magnitude * Time.deltaTime);

            if (Vector2.Distance(transform.position, newPosition) > 2f)
            {
                newPosition = extrapolatedTargetPosition;
                Debug.Log("<color=red>Too big</color>");
            }
            transform.position = newPosition;
        }


        private void InitVairables()
        {
            _rigidbody2D = this.GetComponent<Rigidbody2D>();
            _boxCollider2D = this.GetComponent<BoxCollider2D>();
            _rigidbody2D.gravityScale = Global.DefaultGravity;
            _controllerType = ControllerType.Basic;

        }

        public void InitBasicMovement()
        {

            if (_moveUp != null) _moveUp.Dispose();

            if (_controllerType == ControllerType.ZeroGravity)
            {
                _moveUp = PlayerInput.MoveUpDown
                    .Subscribe(value =>
                    {
                        _rigidbody2D.velocity = new Vector2(0, value * _upForce);
                    })
                    .AddTo(this);
            }
            else
            {
                _moveUp = PlayerInput.MoveUp
                    .Where(_ => PhotonViewManagerOnPlayer.IsPhotonViewMine())
                    .Subscribe(pressingSpace =>
                    {
                        MovementBasenOnControllType(pressingSpace);
                    })
                    .AddTo(this);
            }

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

        public void InvokeStartNewLife()
        {
            PlayerManager.StartNewLife(_rigidbody2D, _controllerType);
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
                    _rigidbody2D.gravityScale = Global.DefaultGravity;
                    break;
                case ControllerType.Reflected:
                    _rigidbody2D.gravityScale = Global.DefaultGravity * -1;
                    break;
                case ControllerType.ZeroGravity:
                    _rigidbody2D.gravityScale = 0;
                    break;
                default:
                    _rigidbody2D.gravityScale = Global.DefaultGravity;
                    break;
            }
        }

        public void SerializeState(PhotonStream stream, PhotonMessageInfo info)
        {

            if (stream.isWriting)
            {
                stream.SendNext((Vector2)transform.position);
                stream.SendNext(_rigidbody2D.velocity);
            }
            else
            {

                currentTime = 0.0;

                _previousInfo.Position = transform.position;
                _previousInfo.Velocity = _networkedPlayerMovementInfo.Velocity;
                _previousInfo.LastNetworkDataReceived = _networkedPlayerMovementInfo.LastNetworkDataReceived;
                _networkedPlayerMovementInfo.LastNetworkDataReceived = info.timestamp;

                _networkedPlayerMovementInfo.Position = (Vector2)stream.ReceiveNext();
                _networkedPlayerMovementInfo.Velocity = (Vector2)stream.ReceiveNext();


            }
        }

    }
}
