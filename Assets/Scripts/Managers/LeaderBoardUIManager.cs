using System;
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
            try
            {
                if (!RestartButton.interactable && PhotonPlayerManager.Instance.IsLocalMaster)
                {
                    RestartButton.interactable = true;
                }

            }
            catch (System.Exception e)
            {

                Debug.LogError(e.Message + Environment.NewLine + e.Data);
                this.enabled = false;
            }
        }

        #endregion
    }
}