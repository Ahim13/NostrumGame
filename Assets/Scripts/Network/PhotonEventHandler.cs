using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{
    public enum PhotonEvents : byte
    {
        SeedSent,
        GameOver
    }

    public class PhotonEventHandler : MonoBehaviour
    {
        void Awake()
        {
            PhotonNetwork.OnEventCall += this.OnSeedArrived;
            PhotonNetwork.OnEventCall += this.OnGameOver;
            Debug.Log("asd");
        }

        private void OnSeedArrived(byte eventCode, object content, int senderID)
        {
            if (eventCode == (byte)PhotonEvents.SeedSent)
            {
                PhotonPlayer sender = PhotonPlayer.Find(senderID);
                if (sender.IsMasterClient)
                {

                    int state = (int)content;
                    RandomSeed.SetSeed(state);
                    Debug.Log("Seed arrived and set to Masters seed");
                }
            }
        }
        private void OnGameOver(byte eventCode, object content, int senderID)
        {
            if (eventCode == (byte)PhotonEvents.GameOver)
            {
                Debug.Log("Game Over");
                Time.timeScale = 0;
            }
            Debug.Log("Why");
        }
    }
}