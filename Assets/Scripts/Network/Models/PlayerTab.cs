using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NostrumGames
{
    public class PlayerTab : MonoBehaviour
    {
        [SerializeField]
        private Text _playerName;
        [SerializeField]
        private Image _selectedCharacter;
        [SerializeField]
        private Button _buttonEdit;
        [SerializeField]
        private Button _buttonKick;

        private int _playerID;

        public string PlayerName { get { return _playerName.text; } }
        public int PlayerID { get { return _playerID; } }


        public void SetPlayerInfo(string playerName, int playerID)
        {
            _playerName.text = playerName;
            _playerID = playerID;
        }

        ///<summary>Set the buttons on playerTab active or not.</summary>
        ///<param name="isMine">Is it my tab?</param>
        ///<param name="isMeMaster">Am I the master?</param>
        public void SetButtonsActive(bool isMine, bool isMeMaster)
        {
            _buttonEdit.gameObject.SetActive(isMine);
            if (!isMine) _buttonKick.gameObject.SetActive(isMeMaster);
        }

        ///<param name="isMine">Is it my tab?</param>
        ///<param name="isMeMaster">Am I the master?</param>
        public void SetPlayerTabAttributes(bool isMine, bool isMeMaster)
        {
            SetButtonsActive(isMine, isMeMaster);
            if (isMine)
            {
                Color mineColor = new Color();
                ColorUtility.TryParseHtmlString("#000080ff", out mineColor);
                _playerName.color = mineColor;
            }
        }
    }
}