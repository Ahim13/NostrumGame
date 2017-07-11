using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;

namespace NostrumGames
{
    public class GiveBlur : Pickups
    {

        public static GiveBlur Instance;

        public override PickupTypes PickupType { get { return PickupTypes.Offensive; } }

        protected override void ActivatePickup()
        {
            _activatePickup = MyInputs.Instance.ActivatePickup
                .Subscribe(_ =>
                {
                    Debug.Log("KILŐŐŐŐŐŐ");
                })
                .AddTo(this);
        }

    }
}
