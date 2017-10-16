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
            Debug.Log("KILŐŐŐŐŐŐ");
            MakeShieldVisible();
            SetPlayerHasShield();

            Destroy(this);
        }

        protected override void LoadPickupSprite()
        {
            PickupSprite = Resources.Load<Sprite>("PickupSprites/1");
        }

        private void SetPlayerHasShield()
        {
            PlayerManager.HasShield = true;
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


