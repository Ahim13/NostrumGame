﻿using System;
using System.Collections;
using System.Collections.Generic;
using Photon;
using UnityEngine;

namespace NostrumGames
{
    public class RoomListingManager : Singleton<RoomListingManager>
    {
        [SerializeField]
        private GameObject _roomListingContainer;
        [SerializeField]
        private GameObject _roomTabPrefab;

        void Awake()
        {
            this.Reload();
        }


        public void RefreshRooms()
        {
            Debug.Log("Refresh");
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