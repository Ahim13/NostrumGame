using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        public void JoinedARoom()
        {
            if (!PhotonPlayerManager.Instance.IsLocalMaster) RoomStartButton.gameObject.SetActive(false);
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
            RoomStartButton.interactable = PhotonPlayerManager.Instance.CheckPlayersCountMoreOrEqual(PlayersNumberOfGame.MinimumPlayersOfPiggyGame, true);
        }
    }
}