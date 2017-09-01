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

        protected override void ActivatePickup()
        {
            _activatePickup = MyInputs.Instance.ActivatePickup
                .Subscribe(_ =>
                {
                    MakeShieldVisible();
                    SetPlayerHasShield();

                    Destroy(this);
                    _activatePickup.Dispose();
                });
        }

        private void SetPlayerHasShield()
        {
            PlayerManager.Instance.HasShield = true;
        }

        private void MakeShieldVisible()
        {
            this.gameObject.AddComponent<SpriteOutline>().outlineSize = ShieldSize;
            Color myColor = new Color();
            ColorUtility.TryParseHtmlString("#33FF00FF", out myColor);
            this.GetComponent<SpriteOutline>().color = myColor;
        }
    }
}


