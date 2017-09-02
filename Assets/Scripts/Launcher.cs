using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

namespace NostrumGames
{
    public class Launcher : PunBehaviour
    {

        [SerializeField]
        private PhotonLogLevel _logLevel = PhotonLogLevel.Informational;
        [SerializeField]
        private byte _maxPlayersPerRoom = 4;

        private string _gameVersion = "1";


        void Awake()
        {
            PhotonNetwork.autoJoinLobby = false;
            PhotonNetwork.automaticallySyncScene = true;
            PhotonNetwork.logLevel = _logLevel;
        }

        void Start()
        {
            Connect();
        }


        private void Connect()
        {
            if (PhotonNetwork.connected)
            {
                PhotonNetwork.JoinRandomRoom();
                Debug.Log("RANDOM ROOM");
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings(_gameVersion);
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Onconnected was called");

            Debug.Log("Rooms: ");

            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("Joined Lobby");
        }

        public override void OnDisconnectedFromPhoton()
        {
            Debug.LogWarning("Disconnected from Photon");
        }

        public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
        {
            Debug.Log("No random room available, so we create one.");

            PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = _maxPlayersPerRoom }, null);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Now this client is in a room.");
            Debug.Log("InLobby: " + PhotonNetwork.insideLobby);
        }
    }
}
