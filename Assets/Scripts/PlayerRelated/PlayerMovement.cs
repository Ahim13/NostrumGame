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

    public class NetworkedPlayerMovementInfo
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public double LastNetworkDataReceived { get; set; }
        public float TimeSinceLastUpdate { get { return (float)(PhotonNetwork.time - LastNetworkDataReceived); } }
        public float SpeedOnY { get { return Velocity.y; } }
        public float SpeedOnX { get { return Velocity.x; } }
    }

    public class PlayerMovement : PlayerBase
    {
        public IObservable<Unit> MovingVelocity { get; private set; }
        public ReactiveProperty<float> ReactiveVelocityX { get; private set; }

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

        private NetworkedPlayerMovementInfo _networkedPlayerMovementInfo;
        private NetworkedPlayerMovementInfo _previousInfo;


        private float lastSynchronizationTime = 0f;
        private float syncDelay = 0f;
        private float syncTime = 0f;
        private float progress = 0f;
        private Vector2 prevPosition = Vector2.zero;
        private Vector2 realPos = Vector2.zero;
        private float timer = 0f;


        public Vector2 realPosition = Vector2.zero;
        public Vector2 positionAtLastPacket = Vector2.zero;
        public double currentTime = 0.0;
        public double currentPacketTime = 0.0;
        public double lastPacketTime = 0.0;
        public double timeToReachGoal = 0.0;


        void Awake()
        {

            if (!PhotonViewManagerOnPlayer.IsPhotonViewMine())
            {
                _networkedPlayerMovementInfo = new NetworkedPlayerMovementInfo();
                return;
            }

            InitVairables();

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

        //Beginner type of prediction implementation to movement TODO: find a better one for specificly 2D plastformer
        private void UpdateNetworkedPostion()
        {

            // if (_previousInfo == null)
            // {
            //     _previousInfo = new NetworkedPlayerMovementInfo();
            // }

            // float pingInSeconds = (float)PhotonNetwork.GetPing() * 0.001f;
            // //float timeSinceLastUpdate = (float)(PhotonNetwork.time - _networkedPlayerMovementInfo.LastNetworkDataReceived);
            // float totalTimePassed = pingInSeconds + _networkedPlayerMovementInfo.TimeSinceLastUpdate;

            // var magnitude = _networkedPlayerMovementInfo.Velocity.magnitude;


            timeToReachGoal = currentPacketTime - lastPacketTime;
            currentTime += Time.deltaTime;
            if (timeToReachGoal != 0) transform.position = Vector2.Lerp(positionAtLastPacket, _networkedPlayerMovementInfo.Position, (float)(currentTime / timeToReachGoal));


            // timer += Time.deltaTime / totalTimePassed;

            // timer = Mathf.Clamp(timer, 0, 1);

            // var newPosition = Vector2.Lerp(_previousInfo.Position, _networkedPlayerMovementInfo.Position, timer);

            // Debug.Log("prevpos: " + _previousInfo.Position);
            // Debug.Log("nextpos: " + _networkedPlayerMovementInfo.Position);
            // Debug.Log("timer: " + timer);
            // Debug.Log("totalTime: " + totalTimePassed);
            // Debug.Log("ping: " + pingInSeconds);
            // Debug.Log("lastupdate: " + _networkedPlayerMovementInfo.TimeSinceLastUpdate);

            // transform.position = newPosition;




        }
        //Beginner type of prediction implementation to movement TODO: find a better one for specificly 2D plastformer
        // private void UpdateNetworkedPostion()
        // {
        //     float pingInSeconds = (float)PhotonNetwork.GetPing() * 0.001f;
        //     float timeSinceLastUpdate = (float)(PhotonNetwork.time - _networkedPlayerMovementInfo.LastNetworkDataReceived);
        //     float totalTimePassed = pingInSeconds + timeSinceLastUpdate;

        //     var magnitude = _networkedPlayerMovementInfo.Velocity.magnitude;

        //     Vector2 extrapolatedTargetPosition = new Vector2(
        //         (_networkedPlayerMovementInfo.Position.x + _networkedPlayerMovementInfo.SpeedOnX * totalTimePassed),
        //         (_networkedPlayerMovementInfo.Position.y + _networkedPlayerMovementInfo.SpeedOnY * totalTimePassed)
        //         );

        //     if (magnitude < 0.1) magnitude = 8;

        //     // Vector2 newPosition = Vector2.MoveTowards(transform.position, _networkedPlayerMovementInfo.Position, magnitude * Time.deltaTime);
        //     Vector2 newPosition = Vector2.Lerp(transform.position, extrapolatedTargetPosition, magnitude * Time.deltaTime);

        //     if (Vector2.Distance(transform.position, newPosition) > 2f)
        //     {
        //         newPosition = extrapolatedTargetPosition;
        //         Debug.Log("<color=red>Too big</color>");
        //     }
        //     transform.position = newPosition;
        // }


        private void InitVairables()
        {
            _rigidbody2D = this.GetComponent<Rigidbody2D>();
            _boxCollider2D = this.GetComponent<BoxCollider2D>();
            _rigidbody2D.gravityScale = Global.DefaultGravity;
            _controllerType = ControllerType.Basic;

        }

        private void InitBasicMovement()
        {

            _moveUp = PlayerInput.MoveUp
                .Where(_ => PhotonViewManagerOnPlayer.IsPhotonViewMine())
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

        //FIXME: Refactor this - it does not belong here
        public void StartNewLife()
        {
            _rigidbody2D.gravityScale = Global.DefaultGravity;
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
                    _rigidbody2D.gravityScale = Global.DefaultGravity;
                    break;
                case ControllerType.Reflected:
                    _rigidbody2D.gravityScale = Global.DefaultGravity * -1;
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
                // _previousInfo = _networkedPlayerMovementInfo;

                currentTime = 0.0;
                positionAtLastPacket = transform.position;
                // realPosition = (Vector2)stream.ReceiveNext();
                lastPacketTime = currentPacketTime;
                currentPacketTime = info.timestamp;

                _networkedPlayerMovementInfo.Position = (Vector2)stream.ReceiveNext();
                _networkedPlayerMovementInfo.Velocity = (Vector2)stream.ReceiveNext();

                _networkedPlayerMovementInfo.LastNetworkDataReceived = info.timestamp;


                timer = 0;

            }
        }

    }
}
