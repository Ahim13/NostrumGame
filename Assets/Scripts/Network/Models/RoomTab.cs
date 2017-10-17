using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NostrumGames
{
    public class RoomTab : MonoBehaviour
    {

        [SerializeField]
        private TextMeshProUGUI _roomNameText;

        [SerializeField]
        private Image _isRoomPrivateImage;


        [SerializeField]
        private TextMeshProUGUI _roomSizeText;

        public string RoomName { get { return _roomNameText.text; } }


        public void SetRoomInfo(string roomNameText, bool isRoomPrivate, string roomSizeText)
        {
            _roomNameText.text = roomNameText;
            _isRoomPrivateImage.enabled = isRoomPrivate;
            _roomSizeText.text = roomSizeText;
        }

        public string GetRoomSizeAndPlayersAsString(byte maxPlayers, int currentPlayers)
        {
            return maxPlayers.ToString() + "/" + currentPlayers.ToString();
        }

        public void DoubleClickedOnTab()
        {
            Debug.Log("DoubleClicked");
            RoomManager.Instance.JoinRoom(this);
        }

        public void SetInteractable(bool interactable)
        {
            this.GetComponent<Button>().interactable = interactable;
        }
    }
}
