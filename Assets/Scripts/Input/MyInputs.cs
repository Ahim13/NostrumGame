using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace NostrumGames
{
    public class MyInputs : MonoBehaviour
    {
        public static MyInputs Instance;

        void Awake()
        {
            SetAsSingleton();

        }

        private void SetAsSingleton()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
        }
    }
}
