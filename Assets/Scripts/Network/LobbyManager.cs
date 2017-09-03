using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using System;

namespace NostrumGames
{
    public class LobbyManager : PunBehaviour
    {
        public static LobbyManager _Instance;

        //TODO: make lobby list for future lobbies
        [SerializeField]
        private Lobby _lobby;

        private TypedLobby _myTypedLobby;

        void Awake()
        {
            SetAsSingleton();
            _myTypedLobby = new TypedLobby(_lobby.LobbyName, _lobby.ThisLobbyType);
        }
        private void SetAsSingleton()
        {
            if (_Instance == null) _Instance = this;
            else if (_Instance != this) Destroy(gameObject);
        }

        public void ConnectToLobby()
        {
            PhotonNetwork.JoinLobby(_myTypedLobby);
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

        #endregion

    }
}
