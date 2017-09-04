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

        private int _playerID;

        public string PlayerName { get { return _playerName.text; } }
        public int PlayerID { get { return _playerID; } }


        public void SetPlayerInfo(string playerName, int playerID)
        {
            _playerName.text = playerName;
            _playerID = playerID;
        }
    }
}