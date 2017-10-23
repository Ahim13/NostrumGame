using System.Collections;
using System.Collections.Generic;
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
            NetworkSceneManager.Instance.LoadScene(sceneName);
        }

    }
}