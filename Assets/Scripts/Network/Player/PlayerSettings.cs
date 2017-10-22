using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

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

        public void SetPlayerProperties(Hashtable hash)
        {
            PhotonNetwork.player.SetCustomProperties(hash);
        }
    }
}