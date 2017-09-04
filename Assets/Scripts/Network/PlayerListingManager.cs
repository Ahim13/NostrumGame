using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{
    public class PlayerListingManager : Singleton<PlayerListingManager>
    {

        [SerializeField]
        private GameObject _playerContainer;
        [SerializeField]
        private GameObject _playerTabPrefab;

        private List<GameObject> _playerTabs;

        void Awake()
        {
            this.Reload();

            _playerTabs = new List<GameObject>();
        }

        public void CreateNewPlayerTab(PhotonPlayer newPlayer)
        {
            var go = Instantiate(_playerTabPrefab);
            go.transform.SetParent(_playerContainer.transform, false);

            var playerTabScript = go.GetComponent<PlayerTab>();
            playerTabScript.SetPlayerInfo(newPlayer.NickName, newPlayer.ID);

            _playerTabs.Add(go);
        }

        //Remove playerTab if same ID as player who left
        public void RemovePlayerTab(PhotonPlayer otherPlayer)
        {
            foreach (var playerTab in _playerTabs)
            {
                var playerTabScript = playerTab.GetComponent<PlayerTab>();
                if (otherPlayer != null && playerTabScript.PlayerID == otherPlayer.ID)
                {
                    Destroy(playerTab);
                    _playerTabs.Remove(playerTab);
                    return;
                }
            }
        }

        public void CreatePlayerTabsForExistingPlayers()
        {
            var playerList = PhotonNetwork.playerList;

            foreach (var player in playerList)
            {
                CreateNewPlayerTab(player);
            }
        }

        public void ClearList()
        {
            foreach (Transform item in _playerContainer.transform)
            {
                Destroy(item.gameObject);
            }
            _playerTabs.Clear();
        }
    }
}
