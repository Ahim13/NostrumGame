using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{


    public class RocketLauncher : Pickups
    {
        public override PickupTypes PickupType { get { return PickupTypes.Offensive; } }

        public override void ActivatePickup()
        {
            _thisPhotonView.RPC("ActivateRocketLaucher", PhotonTargets.Others);
            Destroy(this);
        }

    }
}