using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NostrumGames
{
    public class CreateRoomPanel : MonoBehaviour
    {
        public TMP_InputField RoomName;
        public TMP_InputField MaxPlayers;
        public Toggle IsPrivate;
        public TMP_InputField Password;

        void Awake()
        {
            Password.gameObject.SetActive(false);
        }

        public void MakeRoom()
        {
            try
            {
                RoomManager.Instance.CreateRoom(RoomName.text, Convert.ToByte(MaxPlayers.text), IsPrivate.isOn, Password.text);
                SetPanelActive(false);
            }
            catch (System.NullReferenceException e)
            {
                Debug.Log(e.Message);
                WarningManager.Instance.ShowWarning("No empty boxes!");
            }
            catch (System.FormatException e)
            {
                Debug.Log(e.Message);
                WarningManager.Instance.ShowWarning("No empty boxes!");
            }
            catch (System.Exception e)
            {

                Debug.Log(e.Message);
                WarningManager.Instance.ShowWarning("Room can not be created!");
            }
        }

        public void SetPanelActive(bool active)
        {
            this.gameObject.SetActive(active);
        }

        //TODO: change to click-to-change-number
        public void ValidateMaxPlayerInput(string maxPlayersString)
        {
            int maxPlayers;
            Int32.TryParse(maxPlayersString, out maxPlayers);
            if (maxPlayers < PlayersNumberOfGame.MinimumPlayersOfPiggyGame) maxPlayers = PlayersNumberOfGame.MinimumPlayersOfPiggyGame;
            if (maxPlayers > PlayersNumberOfGame.MaximumPlayersOfPiggyGame) maxPlayers = PlayersNumberOfGame.MaximumPlayersOfPiggyGame;

            maxPlayersString = maxPlayers.ToString();
            MaxPlayers.text = maxPlayersString;
        }

    }
}