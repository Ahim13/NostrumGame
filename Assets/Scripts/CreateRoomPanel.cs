using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NostrumGames
{
    public class CreateRoomPanel : MonoBehaviour
    {
        public InputField RoomName;
        public InputField MaxPlayers;
        public Toggle IsPrivate;
        public InputField Password;

        void Awake()
        {
            Password.gameObject.SetActive(false);
        }

        public void MakeRoom()
        {
            RoomManager._Instance.CreateRoom(RoomName.text, Convert.ToByte(MaxPlayers.text), IsPrivate.isOn, Password.text);
        }

    }
}