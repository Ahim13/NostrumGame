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
        private PlayerController playerController;

        private Vector3 cameraMiddlePosition;
        private Vector3 cameraMiddlePositionLatPos;

        private Sequence deathSequence;
        private Tweener followTween;
        private Tweener setLocalPosTween;

        [SerializeField]
        private float playerStartPointAtX;
        [SerializeField]
        private float delayAfterDeath;
        [SerializeField]
        private float timeToReachSpawnpoint;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            playerController = this.GetComponent<PlayerController>();
            MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            SpeedOnXAxis = playerController.VelocityX;
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
            deathSequence = DOTween.Sequence().Pause();
            // deathSequence.AppendInterval(delayAfterDeath);
            deathSequence.Append(DOTween.ToAlpha(() => renderer.material.color, x => renderer.material.color = x, 0, 0.25f).SetLoops(4, LoopType.Yoyo));

            setLocalPosTween = transform.DOLocalMove(new Vector3(playerStartPointAtX, 0, 10), timeToReachSpawnpoint).SetAutoKill(false).Pause().OnComplete(() => OnSpawnPointSet());
        }

        private void OnSpawnPointSet()
        {
            deathSequence.Rewind();
            // followTween.Rewind();
            // followTween.Pause();
            playerController.StartNewLife();
            transform.SetParent(null);
        }

        private void OnDeath()
        {
            playerController.KillController();
            DOVirtual.DelayedCall(1, () => SetParenting());
            // deathSequence.Play();
            // followTween.SetDelay(delayAfterDeath).Play();
        }


        private void SetParenting()
        {
            transform.SetParent(MainCamera.transform);
            playerController.IsKinematic(true);
            setLocalPosTween.ChangeStartValue(transform.localPosition, timeToReachSpawnpoint);
            setLocalPosTween.Play();
            deathSequence.Play();
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

            if (Input.GetKeyDown(KeyCode.F)) setLocalPosTween.Play();
            if (Input.GetKeyDown(KeyCode.G)) setLocalPosTween.Pause();
            if (Input.GetKeyDown(KeyCode.E)) setLocalPosTween.Restart();
            if (Input.GetKeyDown(KeyCode.R)) setLocalPosTween.Rewind();
            if (Input.GetKeyDown(KeyCode.T)) setLocalPosTween.ChangeStartValue(transform.localPosition, 1);

        }

    }
}
