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
            AudioManager.Instance.PlaySound(Global.RocketLaunch);
            _thisPhotonView.RPC("ActivateRocketLaucher", PhotonTargets.Others);
            Destroy(this);
        }

    }
}