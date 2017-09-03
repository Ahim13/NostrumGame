using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{
    public class UIManager : Singleton<UIManager>
    {
        public GameObject RoomListView;
        public GameObject RoomView;

        void Awake()
        {
            this.Reload();
        }
        public void SwapListViewToRoomView()
        {
            RoomListView.SetActive(!RoomListView.activeSelf);
            RoomView.SetActive(!RoomView.activeSelf);
        }
    }
}