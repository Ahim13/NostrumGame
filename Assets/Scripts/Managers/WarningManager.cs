using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace NostrumGames
{

    public class WarningManager : MonoBehaviour
    {

        public static WarningManager Instance;

        public TextMeshProUGUI _warningText;
        public FadeOutTextEffect _fadeOutEffect;
        public GameObject _warningGameobject;


        void Awake()
        {
            SetAsSingleton();
        }

        private void SetAsSingleton()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
        }

        public void ShowWarning(string warningText)
        {
            _warningText.text = warningText;

            _warningGameobject.SetActive(true);
        }

    }
}