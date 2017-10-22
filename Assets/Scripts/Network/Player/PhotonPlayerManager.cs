﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using System;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace NostrumGames
{
    public class PhotonPlayerManager : PunBehaviour
    {
        public static PhotonPlayerManager Instance;

        public PhotonPlayer LocalPlayer { get { return PhotonNetwork.player; } }
        public PhotonPlayer[] PlayerList { get { return PhotonNetwork.playerList; } }
        public bool IsLocalMaster { get { return PhotonNetwork.isMasterClient; } }

        private static int PlayersInGame = 0;
        private PhotonView _photonView;

        void Awake()
        {
            SetAsSingleton();

            _photonView = GetComponent<PhotonView>();
        }
        private void SetAsSingleton()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
        }
        public bool CheckPlayersCountMoreOrEqual(int number, bool more)
        {
            if (more) return PlayerList.Length >= number;
            else return PlayerList.Length == number;
        }


        public void KickPlayer(PhotonPlayer playerToKick)
        {
            PhotonNetwork.CloseConnection(playerToKick);
        }

        public PhotonPlayer GetPlayer(int ID)
        {
            return PhotonPlayer.Find(ID);
        }

        public void UpdatePlayerProperty<T>(string key, T value)
        {
            var customPropHash = new Hashtable();
            customPropHash.Add(key, value);

            PlayerSettings.Instance.SetPlayerProperties(customPropHash);
        }

        #region RPC Calls

        [PunRPC]
        private void RPC_LoadedGameScene()
        {
            PlayersInGame++;

            //if all players have loaded the scene
            if (CheckPlayersCountMoreOrEqual(PlayersInGame, false))
            {
                RoomManager.Instance.SaveNumberOfPlayersToRoomSettings(PlayersInGame);

                PiggySceneManager.IsGameStarted = true;

                _photonView.RPC("RPC_StartCountBack", PhotonTargets.AllViaServer);
            }
        }
        [PunRPC]
        private void RPC_StartCountBack()
        {
            PiggySceneManager.Instance.StartCountBack();
        }

        #endregion


        #region PUN Callbacks
        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            // Debug.Log("New player: " + newPlayer.NickName);
            PlayerListingManager.Instance.AddPlayerTab(newPlayer);
            if (CheckPlayersCountMoreOrEqual(PlayersNumberOfGame.MinimumPlayersOfPiggyGame, true)) LobbyUIManager.Instance.SetRoomStartButtonInteractable(true);

            //Sync random seed between players - generated by master = send master seed to joined player
            SendMastersSeedToNewPlayer(RandomSeed.GetSeed(), newPlayer);
        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            // Debug.Log("Player left: " + otherPlayer.NickName);

            //FIXME: null érték mert már destroyolodott mikor game scenben hívódik - kijavítani
            if (PlayerListingManager.Instance != null) PlayerListingManager.Instance.RemovePlayerTab(otherPlayer);
        }

        public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
        {
            //check if we are the new Mater client
            if (newMasterClient.ID == PhotonNetwork.player.ID)
            {
                // Debug.Log("<color=blue>We are the master now!</color>");
                if (Scenes.PiggySceneName != SceneManager.GetActiveScene().name) LobbyUIManager.Instance.SetStartButtonActiveAndInteractable();
            }
        }

        public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
        {
            var player = (PhotonPlayer)playerAndUpdatedProps[0];
            var hashTable = (Hashtable)playerAndUpdatedProps[1];

        }

        #endregion

        private void SendMastersSeedToNewPlayer(int seed, PhotonPlayer newPlayer)
        {
            byte eventCode = (byte)PhotonEvents.SeedSent;
            bool reliable = true;
            RaiseEventOptions op = new RaiseEventOptions();
            op.TargetActors = new int[1] { newPlayer.ID };

            PhotonNetwork.RaiseEvent(eventCode, seed, reliable, op);

            // Debug.Log("<color=red>Sent Seed to new player  </color>" + newPlayer + " " + seed);
        }

        public void ClientLoaded()
        {
            _photonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient);
        }

        public void MasterLoaded()
        {
            _photonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient);
        }
    }
}