using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NostrumGames
{
    public class Room : MonoBehaviour
    {

        [SerializeField]
        private Text _roomNameText;

        [SerializeField]
        private Image _isRoomPrivateImage;


        [SerializeField]
        private Text _roomSizeText;


        public void SetRoomInfo(string roomNameText, bool isRoomPrivate, string roomSizeText)
        {
            _roomNameText.text = roomNameText;
            _isRoomPrivateImage.enabled = isRoomPrivate;
            _roomSizeText.text = roomSizeText;
        }

        public string GetRoomSizeAndPlayersAsString(int maxPlayers, int currentPlayers)
        {
            return maxPlayers.ToString() + "/" + currentPlayers.ToString();
        }


    }
}
