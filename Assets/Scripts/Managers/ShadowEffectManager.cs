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
    public class ShadowEffectManager : MonoBehaviour
    {
        public static ShadowEffectManager Instance;

        [SerializeField]
        private float _delayToDisable;
        [SerializeField]
        private GameObject _filterCamera;
        [SerializeField]
        private GameObject _circleSprite;

        void Awake()
        {
            SetAsSingleton();
        }
        private void SetAsSingleton()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
        }

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



