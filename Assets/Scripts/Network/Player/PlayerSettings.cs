using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{
    public class PlayerSettings : MonoBehaviour
    {
        public static PlayerSettings Instance;

        void Awake()
        {
            SetAsSingleton();
        }
        private void SetAsSingleton()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
        }

        public void SetPlayerName(string newName)
        {
            PhotonNetwork.playerName = newName;
        }
    }
}