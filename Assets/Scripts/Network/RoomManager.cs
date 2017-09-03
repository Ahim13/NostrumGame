﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using System;

namespace NostrumGames
{
    public class RoomManager : PunBehaviour
    {
        public static RoomManager _Instance;

        void Awake()
        {
            SetAsSingleton();
        }

        private void SetAsSingleton()
        {
            if (_Instance == null) _Instance = this;
            else if (_Instance != this) Destroy(gameObject);
        }

        public void JoinRoom(RoomTab room)
        {
            PhotonNetwork.JoinRoom(room.RoomName);
        }

        public void CreateRoom(string roomName, byte maxPlayers, bool isSecret, string password)
        {
            RoomOptions roomOptions = new RoomOptions();
            SetRoomSettings(roomOptions, isSecret, password, maxPlayers);

            //Create then join room if possible then load RoomUI
            if (PhotonNetwork.CreateRoom(roomName, roomOptions, null))
            {
                Debug.Log("Room creation sent");
            }
            else
            {
                Debug.Log("Could not create room / room already exist");
            }
        }

        private void SetRoomSettings(RoomOptions roomOptions, bool isSecret, string password, byte maxPlayers)
        {

            string[] customProperties = { "isSecret", "password" };

            ExitGames.Client.Photon.Hashtable customPropHash = new ExitGames.Client.Photon.Hashtable();
            customPropHash.Add("isSecret", isSecret);
            customPropHash.Add("password", password);

            roomOptions.MaxPlayers = maxPlayers;
            roomOptions.PlayerTtl = 1000;
            roomOptions.EmptyRoomTtl = 100;
            roomOptions.CustomRoomPropertiesForLobby = customProperties;
            roomOptions.CustomRoomProperties = customPropHash;
        }

        public void JoinRandomRoom()
        {
            PhotonNetwork.JoinRandomRoom();
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
            Debug.Log("Room created");
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Joined to room: " + PhotonNetwork.room.Name);
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
        #endregion
    }
}