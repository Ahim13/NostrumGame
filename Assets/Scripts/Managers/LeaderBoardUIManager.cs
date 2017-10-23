using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NostrumGames
{

    public class LeaderBoardUIManager : MonoBehaviour
    {

        public Button RestartButton;

        #region Unity Methods

        void Awake()
        {

        }


        void Update()
        {
            if (!RestartButton.interactable && PhotonPlayerManager.Instance.IsLocalMaster)
            {
                RestartButton.interactable = true;
            }
        }

        #endregion
    }
}