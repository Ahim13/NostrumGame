using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{
    public class Relive : Pickups
    {
        public override PickupTypes PickupType { get { return PickupTypes.Relive; } }

        public override void ActivatePickup()
        {
            Debug.Log("Relive");
        }

        protected override void LoadPickupSprite()
        {
            PickupSprite = Resources.Load<Sprite>("PickupSprites/4");
        }
    }
}