using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using DG.Tweening;

namespace NostrumGames
{
    public class PlayerTween : PlayerManager
    {
        private Transform _transform;
        private Renderer _renderer;
        private Camera _mainCamera;


        private Sequence _deathSequence;
        private Tweener _followTween;
        private Tweener _setLocalPosTween;
        private Tweener _outlineTweener;
        private float _timeToReachSpawnpoint;
        private PlayerController _playerController;

        public PlayerTween(Transform trans, Renderer rend, Camera mainCamera, PlayerController playerController)
        {
            this._transform = trans;
            this._renderer = rend;
            this._mainCamera = mainCamera;
            this._playerController = playerController;
        }

        public void TweenInit(float playerStartPointAtX, float timeToReachSpawnpoint)
        {
            _timeToReachSpawnpoint = timeToReachSpawnpoint;

            _deathSequence = DOTween.Sequence().Pause().SetAutoKill(false);
            _deathSequence.Append(DOTween.ToAlpha(() => _renderer.material.color, x => _renderer.material.color = x, 0, 0.25f).SetLoops(4, LoopType.Yoyo));

            _setLocalPosTween = _transform.DOLocalMove(new Vector3(playerStartPointAtX, 0, 10),
            _timeToReachSpawnpoint).SetAutoKill(false).Pause().OnComplete(() => OnSpawnPointSet());
        }

        private void OnSpawnPointSet()
        {
            _deathSequence.Rewind();
            _playerController.StartNewLife();
            _transform.SetParent(null);
            PlayerManager.Instance.IsLiving = true;
        }

        public void OnDeath(PlayerController playerController, float delayAfterDeath)
        {
            _playerController.KillController();
            PlayerManager.Instance.IsLiving = false;
            DOVirtual.DelayedCall(1, () => SetParenting());
        }

        public void OnLoseShield(SpriteOutline outline)
        {
            if (_outlineTweener != null && _outlineTweener.IsPlaying()) return;
            Debug.Log("asdasdsd");
            _outlineTweener = DOTween.To(() => outline.outlineSize, x => outline.outlineSize = x, 0, 2)
                .SetEase(Ease.Flash, 7, 0)
                .OnComplete(() => PlayerManager.Instance.DeleteOutlineComponent());
        }


        private void SetParenting()
        {
            _transform.SetParent(_mainCamera.transform);
            _playerController.IsKinematic(true);
            _setLocalPosTween.ChangeStartValue(_transform.localPosition, _timeToReachSpawnpoint);
            _setLocalPosTween.Play();
            _deathSequence.Play();
        }

    }

}
