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
            Debug.Log("Confuse");
            // _thisPhotonView.RPC("ActivateConfuse", PhotonTargets.All);
            Array.ForEach(_othersPhotonViews, view => view.RPC("ActivateConfuse", PhotonTargets.Others));

            Destroy(this);
        }

    }
}
