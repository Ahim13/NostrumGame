using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NostrumGames
{
    public class RoomListingManager : MonoBehaviour
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
            RemoveAllRoomTabs();

            RoomInfo[] rooms = PhotonNetwork.GetRoomList();
            // Debug.Log("Refresh - Romms: " + rooms.Length);
            // Rooms = new List<RoomInfo>(rooms);

            foreach (var room in rooms)
            {
                AddRoomTab(room);
            }
        }

        public void AddRoomTab(RoomInfo roomInfo)
        {
            var go = Instantiate(_roomTabPrefab);
            go.transform.SetParent(_roomListingContainer.transform, false);
            var roomTab = go.GetComponent<RoomTab>();

            roomTab.SetRoomInfo(roomInfo.Name, (bool)roomInfo.CustomProperties["isSecret"], roomTab.GetRoomSizeAndPlayersAsString(roomInfo.MaxPlayers, roomInfo.PlayerCount));
        }

        private void RemoveAllRoomTabs()
        {
            foreach (Transform child in _roomListingContainer.transform)
            {
                if (child.tag != "Header") GameObject.Destroy(child.gameObject);
            }
        }

    }
}