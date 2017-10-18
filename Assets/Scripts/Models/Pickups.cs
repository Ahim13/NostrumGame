using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

namespace NostrumGames
{
    public enum PickupTypes
    {
        Defensive,
        Offensive,
        Revive
    }

    [RequireComponent(typeof(PlayerManager))]
    [RequireComponent(typeof(PlayerInput))]
    abstract public class Pickups : MonoBehaviour
    {


        public abstract PickupTypes PickupType { get; }

        public Sprite PickupSprite;

        protected IDisposable _activatePickup;

        protected PlayerManager _playerManager;
        protected PlayerInput _playerInput;

        public Pickups()
        {

        }


        void Awake()
        {
            this._playerManager = GetComponent<PlayerManager>();
            this._playerInput = GetComponent<PlayerInput>();
        }


        void Start()
        {
            ActivatePickupOnInput();
        }

        protected void ActivatePickupOnInput()
        {
            _activatePickup = _playerInput.ActivatePickup
                .Subscribe(_ =>
                {
                    ActivatePickup();
                    PickupUIManager.Instance.SetImagesToTransparent();
                })
                .AddTo(this);
        }
        public abstract void ActivatePickup();

        void OnDestroy()
        {
            _activatePickup.Dispose();
        }

    }
}
