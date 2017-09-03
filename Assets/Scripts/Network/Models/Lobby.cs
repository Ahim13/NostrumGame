using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{

    [System.Serializable]
    public class Lobby
    {
        [SerializeField]
        private string _lobbyName;
        [SerializeField]
        private LobbyType _thisLobbyType;

        public string LobbyName { get { return _lobbyName; } }
        public LobbyType ThisLobbyType { get { return _thisLobbyType; } }

    }
}