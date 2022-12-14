using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using DG.Tweening;

namespace NostrumGames
{
    public class PlayerTween
    {
        private Transform _transform;
        private Renderer _renderer;
        private Camera _mainCamera;

        private PlayerMovement _playerMovement;
        private PlayerManager _playerManager;

        private Sequence _deathSequence;
        private Tweener _followTween;
        private Tweener _setLocalPosTween;
        private Tweener _outlineTweener;
        private float _timeToReachSpawnpoint;
        private float _loseShieldDuration = 1.4f;

        public PlayerTween(Transform trans, Renderer rend, Camera mainCamera, PlayerMovement playerController, PlayerManager playerManager)
        {
            this._transform = trans;
            this._renderer = rend;
            this._mainCamera = mainCamera;
            this._playerMovement = playerController;
            this._playerManager = playerManager;
        }

        public void TweenInit(float playerStartPointAtX, float timeToReachSpawnpoint)
        {
            _timeToReachSpawnpoint = timeToReachSpawnpoint;

            _deathSequence = DOTween.Sequence().Pause().SetAutoKill(false);
            _deathSequence.Append(DOTween.ToAlpha(() => _renderer.material.color, x => _renderer.material.color = x, 0, 0.25f).SetLoops(4, LoopType.Yoyo));

            _setLocalPosTween = _transform.DOLocalMove(new Vector3(playerStartPointAtX, 0, 10),
            _timeToReachSpawnpoint).SetAutoKill(false).Pause().OnComplete(() => OnSpawnPointSet());
        }

        /// <summary>
        /// On player postion set to spawn point
        /// </summary>
        private void OnSpawnPointSet()
        {
            _deathSequence.Rewind();
            _playerMovement.InvokeStartNewLife();
            _transform.SetParent(null);
            _playerManager.IsLiving = true;
        }

        public void OnDeath(float delayAfterDeath)
        {
            _playerMovement.KillController();

            if (_playerManager.LivesCounter.HasRemainingLife.Value) DOVirtual.DelayedCall(delayAfterDeath, () => SetParentingStartReposition());
        }

        public void OnLoseShield(SpriteOutline outline)
        {
            if (_outlineTweener != null && _outlineTweener.IsPlaying()) return;
            _outlineTweener = DOTween.To(() => outline.outlineSize, x => outline.outlineSize = x, 0, _loseShieldDuration)
                .SetEase(Ease.Flash, 7, 0)
                .OnComplete(() => _playerManager.DeleteOutlineComponent());
        }


        public void SetParentingStartReposition()
        {
            _transform.SetParent(_mainCamera.transform);
            _playerMovement.IsKinematic(true);
            _setLocalPosTween.ChangeStartValue(_transform.localPosition, _timeToReachSpawnpoint);
            _setLocalPosTween.Play();
            _deathSequence.Play();
            _playerMovement.IsCollider2DEnabled(false);
        }

    }

}
