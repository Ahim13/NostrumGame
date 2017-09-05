using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using System;

namespace NostrumGames
{
    public class PhotonPlayerManager : PunBehaviour
    {
        public static PhotonPlayerManager Instance;

        public PhotonPlayer LocalPlayer { get { return PhotonNetwork.player; } }
        public PhotonPlayer[] PlayerList { get { return PhotonNetwork.playerList; } }

        void Awake()
        {
            SetAsSingleton();
        }
        private void SetAsSingleton()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
        }
        public bool CheckPlayersCountMoreOrEqual(int number)
        {
            return PhotonNetwork.playerList.Length >= number;
        }
        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            Debug.Log("New player: " + newPlayer.NickName);
            PlayerListingManager.Instance.AddPlayerTab(newPlayer);
            if (CheckPlayersCountMoreOrEqual(PlayersNumberOfGame.MinimumPlayersOfPiggyGame)) UIManager.Instance.SetRoomStartButtonInteractable(true);
        }


        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            Debug.Log("Player left: " + otherPlayer.NickName);
            PlayerListingManager.Instance.RemovePlayerTab(otherPlayer);
        }

        public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
        {
            //check if we are the new Mater client
            if (newMasterClient.ID == PhotonNetwork.player.ID)
            {
                Debug.Log("<color=blue>We are the master now!</color>");
                UIManager.Instance.SetStartButtonActiveAndInteractable();
            }
        }
    }
}