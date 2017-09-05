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

        public void SetPlayerName(string newName)
        {
            PhotonNetwork.playerName = newName;
        }
    }
}