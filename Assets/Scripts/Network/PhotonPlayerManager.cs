﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using System;

namespace NostrumGames
{
    public class PhotonPlayerManager : PunBehaviour
    {
        public static PhotonPlayerManager Instance;
        void Awake()
        {
            SetAsSingleton();
        }
        private void SetAsSingleton()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
        }
        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            Debug.Log("New player: " + newPlayer.NickName);
            PlayerListingManager.Instance.CreateNewPlayerTab(newPlayer);
            if (CheckPlayersCountMoreOrEqual(PlayersNumberOfGame.MinimumPlayersOfPiggyGame)) UIManager.Instance.SetRoomStartButtonInteractable(true);
        }


        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            Debug.Log("Player left: " + otherPlayer.NickName);
            PlayerListingManager.Instance.RemovePlayerTab(otherPlayer);
        }
        private bool CheckPlayersCountMoreOrEqual(int number)
        {
            return PhotonNetwork.playerList.Length >= number;
        }
    }
}