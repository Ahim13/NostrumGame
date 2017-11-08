using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using System;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace NostrumGames
{
    public class RoomManager : PunBehaviour
    {
        public static RoomManager Instance;


        [SerializeField]
        [Tooltip("This number if 0 then eveyone has to die to go to the leaderboards")]
        private int NumberOfPlayersToEndGame = 1;

        void Awake()
        {
            SetAsSingleton();
        }

        private void SetAsSingleton()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
        }

        public void JoinRoom(RoomTab room)
        {
            var selectedRoom = PhotonNetwork.GetRoomList().Where(roomInfo => roomInfo.Name.Equals(room.RoomName)).SingleOrDefault();
            if (selectedRoom != null)
            {
                if ((bool)selectedRoom.CustomProperties[RoomProperty.IsSecret])
                {
                    LobbyUIManager.Instance.NeedPassword(selectedRoom);
                    // if (LobbyUIManager.Instance.CheckPassword((string)selectedRoom.CustomProperties[RoomProperty.Password]))
                    // {
                    //     PhotonNetwork.JoinRoom(selectedRoom.Name);
                    // }
                }
                else
                {
                    PhotonNetwork.JoinRoom(selectedRoom.Name);
                }
            }
        }
        public void JoinRandomRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public void CreateRoom(string roomName, byte maxPlayers, bool isSecret, string password)
        {
            RoomOptions roomOptions = new RoomOptions();
            SetRoomSettings(roomOptions, isSecret, password, maxPlayers);

            //Create then join room if possible then load RoomUI
            if (PhotonNetwork.CreateRoom(roomName, roomOptions, null))
            {
                // Debug.Log("Room creation sent");
            }
            else
            {
                // Debug.Log("Could not create room / room already exist");
            }
        }

        private void SetRoomSettings(RoomOptions roomOptions, bool isSecret, string password, byte maxPlayers)
        {

            string[] customProperties = { RoomProperty.IsSecret, RoomProperty.Password };

            Hashtable customPropHash = new Hashtable();
            customPropHash.Add(RoomProperty.IsSecret, isSecret);
            customPropHash.Add(RoomProperty.Password, password);

            roomOptions.MaxPlayers = maxPlayers;
            roomOptions.PlayerTtl = 1000;
            roomOptions.EmptyRoomTtl = 100;
            roomOptions.CustomRoomPropertiesForLobby = customProperties;
            roomOptions.CustomRoomProperties = customPropHash;
        }

        public void MakeRoomVisibleAndOpen(bool open)
        {
            PhotonNetwork.room.IsVisible = open;
            PhotonNetwork.room.IsOpen = open;
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #region PUN Callbacks
        public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
        {
            Debug.Log("Could not join to random room - " + codeAndMsg[1]);
        }
        public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
        {
            Debug.Log("Could not create room - " + codeAndMsg[1]);
        }

        public override void OnCreatedRoom()
        {
            LobbyUIManager.Instance.CreatedARoom();
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Joined to room: " + PhotonNetwork.room.Name);
            // LobbyUIManager.Instance.SwapListViewToRoomView();
            LobbyUIManager.Instance.SwapToRoomView();
            LobbyUIManager.Instance.JoinedARoom();
            PlayerListingManager.Instance.AddPlayerTabsForExistingPlayers();

            SetTimescale(Global.PausedTimeScale);

            //if we are Masterclient then init randomSeed //FIXME: add this to custom property
            if (PhotonNetwork.isMasterClient)
            {
                RandomSeed.InitRandomSeed();
            }

        }
        public override void OnLeftRoom()
        {
            Debug.Log("Left room");
            if (PlayerListingManager.Instance != null) PlayerListingManager.Instance.RemoveAllPlayerTab();

            AudioManager.Instance.StopSound(Global.GameMusic);
            AudioManager.Instance.PlaySound(Global.MenuMusic);

            SetTimescale(Global.NormalTimeScale);
            //UIManager.Instance.SwapListViewToRoomView();
        }

        public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
        {
            int code = Convert.ToInt32(codeAndMsg[0]);
            switch (code)
            {
                case ErrorCode.GameFull:
                    WarningManager.Instance.ShowWarning("Room is full!");
                    break;
                case ErrorCode.GameClosed:
                    WarningManager.Instance.ShowWarning("Room is closed!");
                    break;
                case ErrorCode.GameDoesNotExist:
                    break;
                default:
                    Debug.Log("Join failed: " + codeAndMsg[1]);
                    break;
            }
        }

        public override void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
        {
            if (propertiesThatChanged.ContainsKey(RoomProperty.AlivePlayers) && PhotonPlayerManager.Instance.IsLocalMaster) //only master so it will execute only once
            {
                //check if only one player is alive
                var alivePlayers = (int)propertiesThatChanged[RoomProperty.AlivePlayers];

                if (alivePlayers == NumberOfPlayersToEndGame)
                {
                    var eventCode = (byte)PhotonEvents.GameOver;
                    bool reliable = true;
                    RaiseEventOptions options = new RaiseEventOptions();

                    options.Receivers = ReceiverGroup.All;

                    PhotonNetwork.RaiseEvent(eventCode, null, reliable, options);
                }
            }
        }
        #endregion

        private void SetTimescale(float timeScale)
        {
            Time.timeScale = timeScale;
        }
    }
}
