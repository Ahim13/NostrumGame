using System;
using System.Collections;
using System.Collections.Generic;
using Photon;
using UnityEngine;

namespace NostrumGames
{
    public class RoomListingManager : PunBehaviour
    {
        public static RoomListingManager Instance;

        [SerializeField]
        private GameObject _roomListingContainer;
        [SerializeField]
        private GameObject _roomTabPrefab;

        void Awake()
        {
            SetAsSingleton();
        }

        private void SetAsSingleton()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
        }

        public void RefreshRooms()
        {
            ClearRoomTabs();

            RoomInfo[] rooms = PhotonNetwork.GetRoomList();

            foreach (var room in rooms)
            {
                CreateRoomTab(room);
            }
        }

        public void CreateRoomTab(RoomInfo roomInfo)
        {
            var go = Instantiate(_roomTabPrefab);
            go.transform.SetParent(_roomListingContainer.transform, false);
            var roomTab = go.GetComponent<RoomTab>();

            roomTab.SetRoomInfo(roomInfo.Name, (bool)roomInfo.CustomProperties["isSecret"], roomTab.GetRoomSizeAndPlayersAsString(roomInfo.MaxPlayers, roomInfo.PlayerCount));
        }

        private void ClearRoomTabs()
        {
            foreach (Transform child in _roomListingContainer.transform)
            {
                if (child.tag != "Header") GameObject.Destroy(child.gameObject);
            }
        }

    }
}