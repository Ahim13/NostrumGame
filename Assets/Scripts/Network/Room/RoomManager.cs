﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using System;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace NostrumGames
{
    public class RoomManager : PunBehaviour
    {
        public static RoomManager Instance;

        void Awake()
        {
            SetAsSingleton();
        }

        private void SetAsSingleton()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
        }

        public void JoinRoom(RoomTab room)
        {
            PhotonNetwork.JoinRoom(room.RoomName);
        }
        public void JoinRandomRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public void CreateRoom(string roomName, byte maxPlayers, bool isSecret, string password)
        {
            RoomOptions roomOptions = new RoomOptions();
            SetRoomSettings(roomOptions, isSecret, password, maxPlayers);

            //Create then join room if possible then load RoomUI
            if (PhotonNetwork.CreateRoom(roomName, roomOptions, null))
            {
                // Debug.Log("Room creation sent");
            }
            else
            {
                // Debug.Log("Could not create room / room already exist");
            }
        }

        private void SetRoomSettings(RoomOptions roomOptions, bool isSecret, string password, byte maxPlayers)
        {

            string[] customProperties = { RoomProperty.IsSecret, RoomProperty.Password };

            Hashtable customPropHash = new Hashtable();
            customPropHash.Add(RoomProperty.IsSecret, isSecret);
            customPropHash.Add(RoomProperty.Password, password);

            roomOptions.MaxPlayers = maxPlayers;
            roomOptions.PlayerTtl = 1000;
            roomOptions.EmptyRoomTtl = 100;
            roomOptions.CustomRoomPropertiesForLobby = customProperties;
            roomOptions.CustomRoomProperties = customPropHash;
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }


        #region PUN Callbacks
        public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
        {
            Debug.Log("Could not join to random room - " + codeAndMsg[1]);
        }
        public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
        {
            Debug.Log("Could not create room - " + codeAndMsg[1]);
        }

        public override void OnCreatedRoom()
        {
            LobbyUIManager.Instance.CreatedARoom();
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Joined to room: " + PhotonNetwork.room.Name);
            LobbyUIManager.Instance.SwapListViewToRoomView();
            LobbyUIManager.Instance.JoinedARoom();
            PlayerListingManager.Instance.AddPlayerTabsForExistingPlayers();

            //if we are Masterclient then init randomSeed //FIXME: add this to custom property
            RandomSeed.SetRandomSeed();
        }
        public override void OnLeftRoom()
        {
            Debug.Log("Left room");
            if (PlayerListingManager.Instance != null) PlayerListingManager.Instance.RemoveAllPlayerTab();
            //UIManager.Instance.SwapListViewToRoomView();
        }

        public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
        {
            switch ((int)codeAndMsg[0])
            {
                case ErrorCode.GameFull:
                    Debug.LogWarning("Cant Join, game is full");
                    break;
                case ErrorCode.GameClosed:
                    break;
                case ErrorCode.GameDoesNotExist:
                    break;
                default:
                    Debug.Log("Join failed: " + codeAndMsg[1]);
                    break;
            }
        }

        public override void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
        {
            if (propertiesThatChanged.ContainsKey(RoomProperty.AlivePlayers))
            {
                //check if only one player is alive
                var alivePlayers = (int)propertiesThatChanged[RoomProperty.AlivePlayers];

                if (alivePlayers == 1)
                {
                    var eventCode = (byte)PhotonEvents.GameOver;
                    bool reliable = true;


                    PhotonNetwork.RaiseEvent(eventCode, null, reliable, null);
                }
            }
        }
        #endregion
    }
}
