using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;

namespace NostrumGames
{
    public class Confuse : Pickups
    {

        public override PickupTypes PickupType { get { return PickupTypes.Offensive; } }

        protected override void ActivatePickup()
        {
            _activatePickup = PlayerInput.ActivatePickup
                .Subscribe(_ =>
                {
                    Debug.Log("KILŐŐŐŐŐŐ");
                })
                .AddTo(this);
        }

    }
}
