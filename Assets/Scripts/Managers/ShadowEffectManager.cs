using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Prime31;
using UniRx;
using UniRx.Triggers;

namespace NostrumGames
{
    [RequireComponent(typeof(SpriteLightKitImageEffect))]
    public class ShadowEffectManager : Singleton<ShadowEffectManager>
    {
        protected ShadowEffectManager() { }

        [SerializeField]
        private float _delayToDisable;
        [SerializeField]
        private GameObject _filterCamera;
        [SerializeField]
        private GameObject _circleSprite;

        public void ActivateShadow()
        {
            this.GetComponent<SpriteLightKitImageEffect>().enabled = true;
            _filterCamera.SetActive(true);
            DeactivateShadow();
        }

        private void DeactivateShadow()
        {
            DOVirtual.DelayedCall(_delayToDisable, () =>
                        {
                            this.GetComponent<SpriteLightKitImageEffect>().enabled = false;
                            _filterCamera.SetActive(false);
                        });
        }

    }
}



