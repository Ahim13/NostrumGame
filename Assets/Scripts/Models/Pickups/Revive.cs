using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{
    public class Revive : Pickups
    {
        public override PickupTypes PickupType { get { return PickupTypes.Revive; } }

        public override void ActivatePickup()
        {
            Debug.Log("Revive");
            PlayerManager.LocalPlayerGO.GetComponent<PlayerManager>().RevivePlayer();
        }

        protected override void LoadPickupSprite()
        {
            PickupSprite = Resources.Load<Sprite>("PickupSprites/4");
        }
    }
}