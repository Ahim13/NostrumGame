using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using System.Linq;
using UnityEngine.SceneManagement;

namespace NostrumGames
{
    public class LeaderBoardPiggyManager : PunBehaviour
    {
        [SerializeField]
        private List<PhotonPlayer> _photonPlayers;

        [Header("Ranking")]
        [SerializeField]
        private LeaderBoardPlayer _rank1;
        [SerializeField]
        private LeaderBoardPlayer _rank2;
        [SerializeField]
        private LeaderBoardPlayer _rank3;

        #region Unity Methods

        void Awake()
        {
            try
            {
                _photonPlayers = new List<PhotonPlayer>(PhotonPlayerManager.Instance.PlayerList).OrderByDescending(player => player.GetScore()).ToList();
            }
            catch (System.Exception e)
            {

                Debug.LogError(e.Message + System.Environment.NewLine + e.Data);
                this.enabled = false;
            }
        }

        void Start()
        {
            try
            {

                ChangeAttributesOfLeaderBoardPlayer(_rank1, 0);
                if (_photonPlayers.Count > 1) ChangeAttributesOfLeaderBoardPlayer(_rank2, 1);
                else _rank2.gameObject.SetActive(false);
                if (_photonPlayers.Count > 2) ChangeAttributesOfLeaderBoardPlayer(_rank3, 2);
                else _rank3.gameObject.SetActive(false);
            }
            catch (System.Exception e)
            {

                Debug.LogError(e.Message + System.Environment.NewLine + e.Data);
                this.enabled = false;
            }

            Debug.Log(_photonPlayers.Count);
        }


        #endregion

        private void ChangeAttributesOfLeaderBoardPlayer(LeaderBoardPlayer player, int index)
        {
            player.ChangeName(_photonPlayers[index].NickName);
            player.ChangeScore(_photonPlayers[index].GetScore());
        }

        public void Leave()
        {
            RoomManager.Instance.LeaveRoom();
            SceneManager.LoadScene("Lobby");
        }

        public void Restart()
        {
            NetworkSceneManager.Instance.LoadScene(Scenes.PiggySceneName);
        }
    }
}