using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{
    public class PlayerSettings : Singleton<PlayerSettings>
    {
        void Awake()
        {
            this.Reload();
        }
        public string Name = Guid.NewGuid().ToString();

        public void SetPlayerSettings()
        {
            PhotonNetwork.playerName = Name;
        }

        public void SetPlayerName(string newName)
        {
            Name = newName;
        }
    }
}