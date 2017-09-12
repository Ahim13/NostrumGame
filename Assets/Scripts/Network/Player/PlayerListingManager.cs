using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{
    public class PlayerListingManager : MonoBehaviour
    {

        public static PlayerListingManager Instance;

        [SerializeField]
        private GameObject _playerContainer;
        [SerializeField]
        private GameObject _playerTabPrefab;

        private List<GameObject> _playerTabs;

        public List<GameObject> PlayerTabs { get { return _playerTabs; } }

        void Awake()
        {
            SetAsSingleton();

            _playerTabs = new List<GameObject>();
        }

        private void SetAsSingleton()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
        }

        public void AddPlayerTab(PhotonPlayer newPlayer)
        {
            var go = Instantiate(_playerTabPrefab);
            go.transform.SetParent(_playerContainer.transform, false);

            var playerTabScript = go.GetComponent<PlayerTab>();
            playerTabScript.SetPlayerInfo(newPlayer.NickName, newPlayer.ID);
            playerTabScript.SetPlayerTabAttributes(newPlayer.IsLocal, PhotonPlayerManager.Instance.LocalPlayer.IsMasterClient);

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

        public void RefreshPlayerList()
        {
            RemoveAllPlayerTab();
            AddPlayerTabsForExistingPlayers();
        }

        public void AddPlayerTabsForExistingPlayers()
        {
            var playerList = PhotonPlayerManager.Instance.PlayerList;

            foreach (var player in playerList)
            {
                AddPlayerTab(player);
            }
        }

        public void RemoveAllPlayerTab()
        {
            foreach (Transform item in _playerContainer.transform)
            {
                Destroy(item.gameObject);
            }
            _playerTabs.Clear();
        }
    }
}
