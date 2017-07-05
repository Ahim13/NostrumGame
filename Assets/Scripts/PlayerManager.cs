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

        [SerializeField]
        private float playerStartPointAtX;

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

            cameraMiddlePosition = MainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height / 2, 15));
            cameraMiddlePositionLatPos = cameraMiddlePosition;
            followTween = transform.DOMove(new Vector2(cameraMiddlePosition.x + playerStartPointAtX, cameraMiddlePosition.y), deathSequence.Duration(true)).SetAutoKill(false).Pause();

        }


        private void TweenInit()
        {
            var renderer = this.GetComponent<Renderer>();
            //cameraMiddlePosition = MainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height / 2, MainCamera.nearClipPlane));
            deathSequence = DOTween.Sequence().Pause();
            // deathSequence.AppendInterval(1);
            // deathSequence.Append(transform.DOMove(new Vector2(cameraMiddlePosition.x + playerStartPointAtX, cameraMiddlePosition.y), 2, false));
            // deathSequence.Join(DOTween.ToAlpha(() => renderer.material.color, x => renderer.material.color = x, 0, 1).SetLoops(2, LoopType.Yoyo));
            deathSequence.Append(DOTween.ToAlpha(() => renderer.material.color, x => renderer.material.color = x, 0, 0.35f).SetLoops(6, LoopType.Yoyo));
            deathSequence.OnComplete(deathSequenceEnded());
        }

        private TweenCallback deathSequenceEnded()
        {
            return new TweenCallback(OnSpawnPointSet);
        }

        private void OnSpawnPointSet()
        {
            deathSequence.Rewind();
            playerController.StartNewLife();
            followTween.Rewind();
        }

        private void OnDeath()
        {
            playerController.KillController();
            deathSequence.Play();
            followTween.Play();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            // cameraMiddlePosition = MainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height / 2, 10));


            cameraMiddlePosition = MainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height / 2, 15));

            if (cameraMiddlePosition != cameraMiddlePositionLatPos)
            {
                followTween.ChangeEndValue(new Vector3(cameraMiddlePosition.x + playerStartPointAtX, cameraMiddlePosition.y, 15), 0.15f, true);
            }

            cameraMiddlePositionLatPos = cameraMiddlePosition;

        }

    }
}
