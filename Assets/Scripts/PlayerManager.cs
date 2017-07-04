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
    public class PlayerManager : MonoBehaviour
    {
        public Camera MainCamera { get; private set; }

        private Vector3 cameraMiddlePosition;
        private Vector3 cameraMiddlePositionLatPos;

        [SerializeField]
        private float playerStartPointAtX;
        private Sequence deathSequence;
        private PlayerController playerController;
        private Tweener followTween;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            playerController = this.GetComponent<PlayerController>();
            MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
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
        }

        private void OnDeath()
        {
            cameraMiddlePosition = MainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height / 2, MainCamera.nearClipPlane));
            cameraMiddlePositionLatPos = cameraMiddlePosition;
            followTween = followTween == null ? transform.DOMove(new Vector2(cameraMiddlePosition.x + playerStartPointAtX, cameraMiddlePosition.y), deathSequence.Duration(true)).SetAutoKill(false) : followTween;
            playerController.KillController();
            deathSequence.Play();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            // cameraMiddlePosition = MainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height / 2, 10));

            if (followTween != null && Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log("asd");
                followTween.ChangeEndValue(new Vector2(175, 50), true).Restart();
            }

            cameraMiddlePosition = MainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height / 2, MainCamera.nearClipPlane));
        }

    }
}
