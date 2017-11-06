using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{

    public class EasyMove : Pickups
    {
        public override PickupTypes PickupType { get { return PickupTypes.Defensive; } }

        public override void ActivatePickup()
        {
            _playerManager.ActivateEasyMovement();
            Destroy(this);
        }
    }
}