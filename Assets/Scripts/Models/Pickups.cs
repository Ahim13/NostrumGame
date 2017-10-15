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
        Tool
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
            LoadPickupSprite();
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

        protected abstract void ActivatePickupOnInput();
        public abstract void ActivatePickup();
        protected abstract void LoadPickupSprite();

    }
}
