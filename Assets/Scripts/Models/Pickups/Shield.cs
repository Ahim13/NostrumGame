using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;

namespace NostrumGames
{
    public class Shield : Pickups
    {

        public static readonly int ShieldSize = 16;

        public override PickupTypes PickupType { get { return PickupTypes.Defensive; } }

        public override void ActivatePickup()
        {
            Debug.Log("Shield");
            SetPlayerHasShield(true);
            MakeShieldVisible();

            Destroy(this);
        }

        private void SetPlayerHasShield(bool hasShield)
        {
            _playerManager.HasShield = hasShield;
        }

        private void MakeShieldVisible()
        {

            _thisPhotonView.RPC("MakeVisibleOnline", PhotonTargets.All, ShieldSize);
        }
    }
}


