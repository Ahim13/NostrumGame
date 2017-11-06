using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace NostrumGames
{
    public class LobbyUIManager : MonoBehaviour
    {
        public static LobbyUIManager Instance;

        [Header("ListView")]
        public GameObject RoomListView;
        public GameObject RoomView;

        [Header("RoomView")]
        public Button RoomStartButton;
        [Header("Password")]
        public GameObject PasswordPanel;
        public TextMeshProUGUI RoomPassword;


        private RoomInfo _selectedRoomInfo;

        void Awake()
        {
            SetAsSingleton();
        }

        private void SetAsSingleton()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
        }

        public void CreatedARoom()
        {
            // Debug.Log("Created ROOM setting:  " + PhotonPlayerManager.Instance.IsLocalMaster);
            if (PhotonPlayerManager.Instance.IsLocalMaster)
            {
                SetStartButtonActiveAndInteractable();
            }
            else RoomStartButton.gameObject.SetActive(false);

        }

        internal void NeedPassword(RoomInfo selectedRoomInfo)
        {
            PasswordPanel.SetActive(true);
            _selectedRoomInfo = selectedRoomInfo;
        }

        public void JoinedARoom()
        {
            if (!PhotonPlayerManager.Instance.IsLocalMaster) RoomStartButton.gameObject.SetActive(false);
        }

        public void CheckPasswordAndJoin()
        {
            var roomPw = _selectedRoomInfo.CustomProperties[RoomProperty.Password].ToString();
            var roomPassword = RoomPassword.text.TrimEnd();

            if ((roomPw).Equals(roomPassword))
            {
                PhotonNetwork.JoinRoom(_selectedRoomInfo.Name);
            }
            else
            {
                WarningManager.Instance.ShowWarning("Wrong password!");
            }
        }

        public void SwapListViewToRoomView()
        {
            RoomListView.SetActive(!RoomListView.activeSelf);
            RoomView.SetActive(!RoomView.activeSelf);
        }
        public void SwapToRoomView()
        {
            RoomListView.SetActive(false);
            RoomView.SetActive(true);
        }

        public void SetRoomStartButtonInteractable(bool interactable)
        {
            RoomStartButton.interactable = interactable;
        }

        ///<summary> Set Active to true and set interactible based on minimum players in room</summary>
        public void SetStartButtonActiveAndInteractable()
        {
            RoomStartButton.gameObject.SetActive(true);
            RoomStartButton.interactable = PhotonPlayerManager.Instance.CheckPlayersCountMoreOrEqual(PlayersNumberOfGame.MinimumPlayersOfPiggyGame, true);
        }
    }
}