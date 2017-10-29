using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{
    public class TnTLauncher : Pickups
    {
        public override PickupTypes PickupType { get { return PickupTypes.Offensive; } }

        public override void ActivatePickup()
        {
            _thisPhotonView.RPC("ActivateTnTLaucher", PhotonTargets.Others);
            Destroy(this);
        }
    }
}