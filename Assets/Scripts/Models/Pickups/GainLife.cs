using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{

    public class GainLife : Pickups
    {
        public override PickupTypes PickupType { get { return PickupTypes.Defensive; } }

        public override void ActivatePickup()
        {
            _playerManager.ActivateGainLife();
            Destroy(this);
        }

    }
}