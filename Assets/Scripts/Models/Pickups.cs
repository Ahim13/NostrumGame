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

        protected IDisposable _activatePickup;

        protected PlayerManager PlayerManager;
        protected PlayerInput PlayerInput;

        void Awake()
        {
            this.PlayerManager = GetComponent<PlayerManager>();
            this.PlayerInput = GetComponent<PlayerInput>();
        }


        void Start()
        {
            ActivatePickup();
        }

        protected abstract void ActivatePickup();

    }
}
