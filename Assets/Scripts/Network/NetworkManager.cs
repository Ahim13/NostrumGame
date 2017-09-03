using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using System;

namespace NostrumGames
{
    public class NetworkManager : PunBehaviour
    {

        [SerializeField]
        private NetworkSettings _networkSettings;

        public static NetworkManager _Instance;

        void Awake()
        {
            SetAsSingleton();
            _networkSettings.PhotonNetworkSettings();
        }

        private void SetAsSingleton()
        {
            if (_Instance == null) _Instance = this;
            else if (_Instance != this) Destroy(gameObject);
        }

        void Start()
        {
            ConnectToMasterServer();
        }

        private void ConnectToMasterServer()
        {
            PhotonNetwork.ConnectUsingSettings(_networkSettings.GameVersion);
        }

        #region PUN Callbacks
        public override void OnConnectedToMaster()
        {
            LobbyManager._Instance.ConnectToLobby();
        }
        public override void OnDisconnectedFromPhoton()
        {
            Debug.LogWarning("Disconnected from Photon");
        }
        #endregion


    }
}