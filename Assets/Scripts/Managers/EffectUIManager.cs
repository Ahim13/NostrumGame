using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace NostrumGames
{
    public class EffectUIManager : MonoBehaviour
    {

        public static EffectUIManager Instance;


        public TextMeshProUGUI EffectDescritpion;

        #region Unity Methods

        void Awake()
        {
            SetAsSingleton();
        }
        private void SetAsSingleton()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
        }

        #endregion

        public void ShowEffectDescription(string desc, float time)
        {
            EffectDescritpion.text = desc;
            EffectDescritpion.gameObject.SetActive(true);
            DOVirtual.DelayedCall(time, () =>
            {
                EffectDescritpion.gameObject.SetActive(false);
            });
        }
    }
}