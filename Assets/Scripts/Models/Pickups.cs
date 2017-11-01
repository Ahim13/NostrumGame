using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using System.Linq;

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
        protected PhotonView _thisPhotonView;
        protected PhotonView[] _othersPhotonViews;

        void Awake()
        {
            this._playerManager = GetComponent<PlayerManager>();
            this._playerInput = GetComponent<PlayerInput>();
            this._thisPhotonView = PhotonView.Get(this);
            this._othersPhotonViews = GameObject.FindGameObjectsWithTag(Global.OtherPlayerTagName).Select(g => g.GetComponent<PhotonView>()).ToArray();
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

        public void UsePickup()
        {
            this._thisPhotonView = GameObject.FindGameObjectWithTag("Player").GetComponent<PhotonView>();
            this._othersPhotonViews = GameObject.FindGameObjectsWithTag(Global.OtherPlayerTagName).Select(g => g.GetComponent<PhotonView>()).ToArray();
            ActivatePickup();
        }

        void OnDestroy()
        {
            _playerManager.RemovePickup();
            _activatePickup.Dispose();
        }

    }
}
