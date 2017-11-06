using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NostrumGames
{
    public class RoomTab : MonoBehaviour, ISelectHandler
    {

        [SerializeField]
        private TextMeshProUGUI _roomNameText;

        [SerializeField]
        private Image _isRoomPrivateImage;

        [SerializeField]
        private TextMeshProUGUI _roomSizeText;
        [SerializeField]
        private Color _selectedColor;
        [SerializeField]
        private Color _unSelectedColor;

        public string RoomName { get { return _roomNameText.text; } }

        public bool IsSelected { get; set; }


        public void SetRoomInfo(string roomNameText, bool isRoomPrivate, string roomSizeText)
        {
            _roomNameText.text = roomNameText;
            _isRoomPrivateImage.enabled = isRoomPrivate;
            _roomSizeText.text = roomSizeText;

            IsSelected = false;
        }

        public string GetRoomSizeAndPlayersAsString(byte maxPlayers, int currentPlayers)
        {
            return maxPlayers.ToString() + "/" + currentPlayers.ToString();
        }

        public void DoubleClickedOnTab()
        {
            //Debug.Log("DoubleClicked");
            RoomManager.Instance.JoinRoom(this);

        }

        public void SetInteractable(bool interactable)
        {
            this.GetComponent<Button>().interactable = interactable;
        }

        void Update()
        {
            if (IsSelected) GetComponent<Image>().color = _selectedColor;
            else GetComponent<Image>().color = _unSelectedColor;
        }
        public void OnSelect(BaseEventData eventData)
        {
            RoomListingManager.Instance.RoomTabs.ForEach(roomTab => roomTab.IsSelected = false);
            IsSelected = true;
        }

    }
}
