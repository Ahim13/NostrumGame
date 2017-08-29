﻿using System;
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


        protected override void ActivatePickup()
        {
            _activatePickup = MyInputs.Instance.ActivatePickup
                .Subscribe(_ =>
                {
                    ShadowEffectManager.Instance.ActivateShadow();
                })
                .AddTo(this);
        }
    }
}



