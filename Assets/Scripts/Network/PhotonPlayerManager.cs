using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

namespace NostrumGames
{
    public class PhotonPlayerManager : PunBehaviour
    {
        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            Debug.Log("New player: " + newPlayer.NickName);
            PlayerListingManager.Instance.CreateNewPlayerTab(newPlayer);
        }
        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            Debug.Log("Player left: " + otherPlayer.NickName);
            PlayerListingManager.Instance.RemovePlayerTab(otherPlayer);
        }
    }
}