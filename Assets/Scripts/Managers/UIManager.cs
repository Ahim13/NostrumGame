﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NostrumGames
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("ListView")]
        public GameObject RoomListView;
        public GameObject RoomView;

        [Header("RoomView")]
        public Button RoomStartButton;

        void Awake()
        {
            this.Reload();
        }

        public void CreatedARoom()
        {
            if (PhotonNetwork.isMasterClient)
            {
                RoomStartButton.gameObject.SetActive(true);
                SetRoomStartButtonInteractable(false);
            }
            else RoomStartButton.gameObject.SetActive(false);

        }

        public void JoinedARoom()
        {
            RoomStartButton.gameObject.SetActive(false);
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

        ///<summary> Set Active to true and set interactible based on minimum players in room</summary>
        public void SetStartButtonActiveAndInteractable()
        {
            RoomStartButton.gameObject.SetActive(true);
            RoomStartButton.interactable = PhotonPlayerManager.Instance.CheckPlayersCountMoreOrEqual(PlayersNumberOfGame.MinimumPlayersOfPiggyGame);
        }
    }
}