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
    public class Pickups : MonoBehaviour
    {


        public virtual PickupTypes PickupType { get; }

        public Sprite PickupSprite;

        protected IDisposable _activatePickup;

        protected PlayerManager _playerManager;
        protected PlayerInput _playerInput;
        protected PhotonView _thisPhotonView;


        void Awake()
        {
            this._playerManager = GetComponent<PlayerManager>();
            this._playerInput = GetComponent<PlayerInput>();
            this._thisPhotonView = PhotonView.Get(this);
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
        public virtual void ActivatePickup() { }

        void OnDestroy()
        {
            _activatePickup.Dispose();
        }

    }
}
