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

    abstract public class Pickups : MonoBehaviour
    {


        public abstract PickupTypes PickupType { get; }

        protected IDisposable _activatePickup;

        protected PlayerManager PlayerManager;

        void Awake()
        {
            this.PlayerManager = GetComponent<PlayerManager>();
        }


        void Start()
        {
            ActivatePickup();
        }

        protected abstract void ActivatePickup();

    }
}
