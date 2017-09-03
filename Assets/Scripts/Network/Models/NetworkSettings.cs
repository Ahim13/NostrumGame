using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{

    [System.Serializable]
    public class NetworkSettings
    {
        public PhotonLogLevel LogLevel = PhotonLogLevel.ErrorsOnly;
        public string GameVersion = "0.1";

        public void PhotonNetworkSettings()
        {
            PhotonNetwork.autoJoinLobby = false;
            PhotonNetwork.automaticallySyncScene = true;
            PhotonNetwork.logLevel = LogLevel;
            PhotonNetwork.EnableLobbyStatistics = true;
        }
    }
}