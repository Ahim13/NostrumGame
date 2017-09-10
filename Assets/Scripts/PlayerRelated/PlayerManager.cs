using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using DG.Tweening;

namespace NostrumGames
{
    public class PlayerManager : PlayerBase, IPunObservable
    {
        protected PlayerManager() { }


        public Camera MainCamera { get; private set; }

        public int Lives { get { return _lives; } set { _lives = value; } }

        public bool IsLiving { get; set; }
        public bool HasShield { get; set; }

        public List<Component> PickupList { get; set; }


        private PlayerTween _playerTween;
        private LivesCounter _livesCounter;


        private Vector3 _cameraMiddlePosition;
        private Vector3 _cameraMiddlePositionLatPos;


        [SerializeField]
        private int _lives;

        [Header("Tween Settings")]
        [SerializeField]
        private float _playerStartPointAtX;
        [SerializeField]
        private float _delayAfterDeath;
        [SerializeField]
        private float _timeToReachSpawnpoint;


        void Awake()
        {
            InitVariablesProperties();
            InitTools();
        }


        void Start()
        {

            if (!PhotonViewManagerOnPlayer.IsPhotonViewMine()) return;

            (this).OnCollisionEnter2DAsObservable()
                .Where(_ => IsLiving)
                .Where(col => col.gameObject.tag == "Map")
                .Subscribe(col =>
                {
                    if (!HasShield) OnCollisionWithObstacle();
                    else LoseShield();
                })
                .AddTo(this);

            _playerTween.TweenInit(_playerStartPointAtX, _timeToReachSpawnpoint);

        }
        void Update()
        {
            // if (Input.GetKeyDown(KeyCode.A)) Debug.Log(HasShield);
            // if (Input.GetKeyDown(KeyCode.S)) Debug.Log(Shield.Value);
        }


        private void InitTools()
        {
            PickupList = new List<Component>();
            _playerTween = new PlayerTween(transform, this.GetComponent<Renderer>(), MainCamera, PlayerMovement, this);
            _livesCounter = new LivesCounter(_lives, this);
        }
        private void LoseShield()
        {
            if (GetComponent<SpriteOutline>()) _playerTween.OnLoseShield(GetComponent<SpriteOutline>());
        }

        public void DeleteOutlineComponent()
        {
            Destroy(GetComponent<SpriteOutline>());
            HasShield = false;
        }


        private void OnCollisionWithObstacle()
        {
            _playerTween.OnDeath(PlayerMovement, _delayAfterDeath);
            _livesCounter.Died();
            if (PickupList.Count > 0) RemovePickup();
        }

        private void InitVariablesProperties()
        {
            IsLiving = true;
            MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            HasShield = false;
        }

        public void RemovePickup()
        {
            Destroy(PickupList[0]);
            PickupList.RemoveAt(0);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            SerializeState(stream, info);

            PlayerMovement.SerializeState(stream, info);
        }

        private void SerializeState(PhotonStream stream, PhotonMessageInfo info)
        {
            //throw new NotImplementedException();
        }
    }
}
