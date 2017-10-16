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
        Relive
    }

    [RequireComponent(typeof(PlayerManager))]
    [RequireComponent(typeof(PlayerInput))]
    abstract public class Pickups : MonoBehaviour
    {


        public abstract PickupTypes PickupType { get; }

        public Sprite PickupSprite;

        protected IDisposable _activatePickup;

        protected PlayerManager PlayerManager;
        protected PlayerInput PlayerInput;

        public Pickups()
        {
            //if (PickupSprite == null) LoadPickupSprite();
        }


        void Awake()
        {
            this.PlayerManager = GetComponent<PlayerManager>();
            this.PlayerInput = GetComponent<PlayerInput>();

        }


        void Start()
        {
            ActivatePickupOnInput();
        }

        protected void ActivatePickupOnInput()
        {
            _activatePickup = PlayerInput.ActivatePickup
                .Subscribe(_ =>
                {
                    ActivatePickup();
                })
                ;
        }
        public abstract void ActivatePickup();
        protected abstract void LoadPickupSprite();

    }
}
