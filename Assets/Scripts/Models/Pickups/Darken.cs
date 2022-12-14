using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;

namespace NostrumGames
{
    public class Darken : Pickups
    {

        public override PickupTypes PickupType { get { return PickupTypes.Offensive; } }

        public override void ActivatePickup()
        {
            _thisPhotonView.RPC("ActivateDarken", PhotonTargets.Others);
            Debug.Log("Darken");

            Destroy(this);
        }
    }
}



