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
        private PlayerController _playerController;

        private Vector3 _cameraMiddlePosition;
        private Vector3 _cameraMiddlePositionLatPos;

        private Sequence _deathSequence;
        private Tweener _followTween;
        private Tweener _setLocalPosTween;

        [SerializeField]
        private float _playerStartPointAtX;
        [SerializeField]
        private float _delayAfterDeath;
        [SerializeField]
        private float _timeToReachSpawnpoint;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            _playerController = this.GetComponent<PlayerController>();
            MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            SpeedOnXAxis = _playerController.VelocityX;
        }

        // Use this for initialization
        void Start()
        {
            this.OnCollisionEnter2DAsObservable()
                .Where(col => col.gameObject.tag == "Map")
                .Subscribe(col =>
                {
                    OnDeath();
                })
                .AddTo(this);

            TweenInit();

            // cameraMiddlePosition = MainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height / 2, 15));
            // cameraMiddlePositionLatPos = cameraMiddlePosition;
            // followTween = transform.DOMove(new Vector2(cameraMiddlePosition.x + playerStartPointAtX, cameraMiddlePosition.y), deathSequence.Duration(true) - delayAfterDeath).SetAutoKill(false).Pause();
        }


        private void TweenInit()
        {
            var renderer = this.GetComponent<Renderer>();
            _deathSequence = DOTween.Sequence().Pause();
            // deathSequence.AppendInterval(delayAfterDeath);
            _deathSequence.Append(DOTween.ToAlpha(() => renderer.material.color, x => renderer.material.color = x, 0, 0.25f).SetLoops(4, LoopType.Yoyo));

            _setLocalPosTween = transform.DOLocalMove(new Vector3(_playerStartPointAtX, 0, 10), _timeToReachSpawnpoint).SetAutoKill(false).Pause().OnComplete(() => OnSpawnPointSet());
        }

        private void OnSpawnPointSet()
        {
            _deathSequence.Rewind();
            // followTween.Rewind();
            // followTween.Pause();
            _playerController.StartNewLife();
            transform.SetParent(null);
        }

        private void OnDeath()
        {
            _playerController.KillController();
            DOVirtual.DelayedCall(1, () => SetParenting());
            // deathSequence.Play();
            // followTween.SetDelay(delayAfterDeath).Play();
        }


        private void SetParenting()
        {
            transform.SetParent(MainCamera.transform);
            _playerController.IsKinematic(true);
            _setLocalPosTween.ChangeStartValue(transform.localPosition, _timeToReachSpawnpoint);
            _setLocalPosTween.Play();
            _deathSequence.Play();
            //DOVirtual.DelayedCall(2, () => OnSpawnPointSet());
        }

        void Update()
        {

            // cameraMiddlePosition = MainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height / 2, 15));

            // if (cameraMiddlePosition != cameraMiddlePositionLatPos)
            // {
            //     followTween.ChangeEndValue(new Vector3(cameraMiddlePosition.x + playerStartPointAtX, cameraMiddlePosition.y, 15), 1, true);
            // }

            // cameraMiddlePositionLatPos = cameraMiddlePosition;

            if (Input.GetKeyDown(KeyCode.F)) _setLocalPosTween.Play();
            if (Input.GetKeyDown(KeyCode.G)) _setLocalPosTween.Pause();
            if (Input.GetKeyDown(KeyCode.E)) _setLocalPosTween.Restart();
            if (Input.GetKeyDown(KeyCode.R)) _setLocalPosTween.Rewind();
            if (Input.GetKeyDown(KeyCode.T)) _setLocalPosTween.ChangeStartValue(transform.localPosition, 1);

        }

    }
}
