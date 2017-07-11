using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

namespace NostrumGames
{
    abstract public class Pickups : MonoBehaviour
    {

        public enum PickupTypes
        {
            Defensive,
            Offensive,
            Tool
        }

        public abstract PickupTypes PickupType { get; }

        protected IDisposable _activatePickup;


        void Start()
        {
            ActivatePickup();
        }

        protected abstract void ActivatePickup();

    }
}
