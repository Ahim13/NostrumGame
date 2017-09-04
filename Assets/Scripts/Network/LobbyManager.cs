using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using System;

namespace NostrumGames
{
    public class LobbyManager : PunBehaviour
    {
        public static LobbyManager Instance;

        //TODO: make lobby list for future lobbies
        [SerializeField]
        private Lobby _lobby;

        private TypedLobby _myTypedLobby;

        void Awake()
        {
            SetAsSingleton();

            //For one game the lobby is set at start -> TODO:More lobby = new Lobbysetting implementation
            _myTypedLobby = new TypedLobby(_lobby.LobbyName, _lobby.ThisLobbyType);
        }
        private void SetAsSingleton()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
        }

        public void ConnectToLobby()
        {
            PhotonNetwork.JoinLobby(_myTypedLobby);
        }

        public void LeaveCurrentLobby()
        {
            PhotonNetwork.LeaveLobby();
            var go = GameObject.FindGameObjectWithTag("RoomTools");
            if (go != null)
            {
                Destroy(go);
                Debug.LogWarning("RoomTools has been destroyed!");
            }
        }

        #region PUN Callbacks
        public override void OnJoinedLobby()
        {
            Debug.Log("InLobby: " + PhotonNetwork.lobby.Name);
        }

        public override void OnLeftLobby()
        {
            Debug.Log("Lobby left");
        }

        public override void OnReceivedRoomListUpdate()
        {
            Debug.Log("ReceivedRoom");
            RoomListingManager.Instance.RefreshRooms();
        }

        #endregion

    }
}
