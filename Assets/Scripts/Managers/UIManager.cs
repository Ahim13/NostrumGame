using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NostrumGames
{
    public class UIManager : Singleton<UIManager>
    {
        public GameObject RoomListView;
        public GameObject RoomView;
        public Button RoomStartButton;

        void Awake()
        {
            this.Reload();
        }
        public void SwapListViewToRoomView()
        {
            RoomListView.SetActive(!RoomListView.activeSelf);
            RoomView.SetActive(!RoomView.activeSelf);
        }

        public void SetRoomStartButtonInteractable(bool interactable)
        {
            RoomStartButton.interactable = interactable;
        }
    }
}