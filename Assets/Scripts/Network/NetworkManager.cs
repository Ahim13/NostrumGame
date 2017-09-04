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

        public static NetworkManager Instance;

        void Awake()
        {
            SetAsSingleton();
            _networkSettings.PhotonNetworkSettings();
        }

        private void SetAsSingleton()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
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
            Debug.Log("Connected to Master");
            PlayerSettings.Instance.SetPlayerSettings();
            LobbyManager.Instance.ConnectToLobby();
        }
        public override void OnDisconnectedFromPhoton()
        {
            Debug.LogWarning("Disconnected from Photon");
        }
        #endregion


    }
}