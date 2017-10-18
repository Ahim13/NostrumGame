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

        public override void ActivatePickup()
        {
            Debug.Log("KILŐŐŐŐŐŐ");
        }

    }
}
