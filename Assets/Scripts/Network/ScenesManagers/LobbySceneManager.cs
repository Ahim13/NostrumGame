using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace NostrumGames
{
    public class LobbySceneManager : MonoBehaviour
    {

        void Awake()
        {
            ApplicationSettings.IsPiggyGameStarted = false;
        }

        public void LeaveRoomButton()
        {
            RoomManager.Instance.LeaveRoom();
            LobbyUIManager.Instance.SwapListViewToRoomView();
        }

        public void StartButtonPressed(string sceneName)
        {
            RoomManager.Instance.MakeRoomVisibleAndOpen(false);
            NetworkSceneManager.Instance.LoadScene(sceneName);
        }

        public void RandomJoinButtonPressed()
        {
            RoomManager.Instance.JoinRandomRoom();
        }

        public void JoinButtonPressed()
        {
            try
            {
                var selectedRoomTab = RoomListingManager.Instance.RoomTabs.Where(roomTab => roomTab.IsSelected).Single();
                selectedRoomTab.DoubleClickedOnTab();

            }
            catch (System.Exception e)
            {
                WarningManager.Instance.ShowWarning("No room selected");

                Debug.LogWarning(e.Message);
            }

        }

    }
}