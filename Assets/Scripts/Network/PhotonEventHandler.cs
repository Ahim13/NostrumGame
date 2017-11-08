using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{
    public enum PhotonEvents : byte
    {
        SeedSent,
        GameOver,
        StartTime
    }

    public class PhotonEventHandler : MonoBehaviour
    {
        void Awake()
        {
            PhotonNetwork.OnEventCall += this.OnSeedArrived;
            PhotonNetwork.OnEventCall += this.OnGameOver;
            PhotonNetwork.OnEventCall += this.OnStartTime;
        }

        private void OnSeedArrived(byte eventCode, object content, int senderID)
        {
            if (eventCode == (byte)PhotonEvents.SeedSent)
            {
                PhotonPlayer sender = PhotonPlayer.Find(senderID);
                if (sender.IsMasterClient)
                {
                    int[] seeds = (int[])content;

                    int newSeed = seeds[0];
                    int newMapSeed = seeds[1];
                    RandomSeed.SetSeed(newSeed);
                    RandomSeed.SetMapSeed(newMapSeed);
                    Debug.Log("Seed arrived and set to Masters seed");
                }
            }
        }
        private void OnGameOver(byte eventCode, object content, int senderID)
        {
            if (eventCode == (byte)PhotonEvents.GameOver)
            {
                Debug.Log("Game Over");
                // Time.timeScale = 0;

                //set score before loading
                // PhotonPlayerManager.Instance.LocalPlayer.SetScore((int)ScoreManager.Instance.Score);

                // NetworkSceneManager.Instance.LoadScene(Scenes.PiggyLeaderBoard);
                StartCoroutine(LoadLeaderboards());
            }
        }
        private void OnStartTime(byte eventCode, object content, int senderID)
        {
            if (eventCode == (byte)PhotonEvents.StartTime)
            {
                var photonTime = PhotonNetwork.time;
                var masterTime = (double)content;
                PhotonPlayerManager.Instance.StartTime = masterTime;
            }
        }

        IEnumerator LoadLeaderboards()
        {
            PhotonPlayerManager.Instance.LocalPlayer.SetScore((int)ScoreManager.Instance.Score);
            yield return null;
            NetworkSceneManager.Instance.LoadScene(Scenes.PiggyLeaderBoard);
        }
    }
}