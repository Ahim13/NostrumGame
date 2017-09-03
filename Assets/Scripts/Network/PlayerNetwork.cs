using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{
    public class PlayerNetwork : Singleton<PlayerNetwork>
    {

        public string Name { get; private set; }

        void Awake()
        {
            Name = "Ben" + Guid.NewGuid().ToByteArray();
            this.Reload();

            PhotonNetwork.playerName = Name;
        }
    }
}