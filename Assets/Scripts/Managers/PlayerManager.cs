using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using DG.Tweening;

namespace NostrumGames
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerManager : Singleton<PlayerManager>
    {
        protected PlayerManager() { }

        public Camera MainCamera { get; private set; }
        public float SpeedOnXAxis { get; private set; }
        public int Lives { get { return _lives; } set { _lives = value; } }
        public bool IsLiving { get; set; }
        public bool HasShield { get; set; }
        public ReactiveProperty<bool> Shield { get; private set; }
        public List<Component> PickupList { get; set; }


        private PlayerController _playerController;
        private PlayerTween _playerTween;
        private LivesCounter _livesCounter;


        private Vector3 _cameraMiddlePosition;
        private Vector3 _cameraMiddlePositionLatPos;


        [SerializeField]
        private int _lives;

        [Space(15)]
        [SerializeField]
        private float _playerStartPointAtX;
        [SerializeField]
        private float _delayAfterDeath;
        [SerializeField]
        private float _timeToReachSpawnpoint;



        void Awake()
        {
            this.Reload();

            InitVariablesProperties();
            InitTools();

            Shield = this.UpdateAsObservable()
                .Select(_ => HasShield)
                .ToReactiveProperty();
        }


        void Start()
        {
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
            if (Input.GetKeyDown(KeyCode.A)) Debug.Log(HasShield);
            if (Input.GetKeyDown(KeyCode.S)) Debug.Log(Shield.Value);
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
            _playerTween.OnDeath(_playerController, _delayAfterDeath);
            _livesCounter.Died();
            if (PickupList.Count > 0) RemovePickup();
        }

        private void InitVariablesProperties()
        {
            _playerController = this.GetComponent<PlayerController>();
            IsLiving = true;
            MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            SpeedOnXAxis = _playerController.VelocityX;
            HasShield = false;
        }

        private void InitTools()
        {
            PickupList = new List<Component>();
            _playerTween = new PlayerTween(transform, this.GetComponent<Renderer>(), MainCamera, _playerController);
            _livesCounter = new LivesCounter(_lives);
        }

        public void RemovePickup()
        {
            Destroy(PickupList[0]);
            PickupList.RemoveAt(0);
        }

    }
}
