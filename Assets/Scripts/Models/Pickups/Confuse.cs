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
            Debug.Log("KILŐŐŐŐŐŐ");
        }

        protected override void ActivatePickupOnInput()
        {
            _activatePickup = PlayerInput.ActivatePickup
                .Subscribe(_ =>
                {
                    ActivatePickup();
                })
                .AddTo(this);
        }


        protected override void LoadPickupSprite()
        {
            PickupSprite = Resources.Load<Sprite>("PickupSprites/2");
        }

    }
}
