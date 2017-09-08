using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(PlayerManager))]
    [RequireComponent(typeof(PhotonViewManagerOnPlayer))]
    public class PlayerBase : MonoBehaviour
    {
        PlayerController _playerController;
        public PlayerController PlayerController
        {
            get
            {
                if (_playerController == null) _playerController = GetComponent<PlayerController>();
                return _playerController;
            }
        }

        PlayerManager _playerManager;
        public PlayerManager PlayerManager
        {
            get
            {
                if (_playerManager == null) _playerManager = GetComponent<PlayerManager>();
                return _playerManager;
            }
        }

        PhotonViewManagerOnPlayer _photonViewManagerOnPlayer;
        public PhotonViewManagerOnPlayer PhotonViewManagerOnPlayer
        {
            get
            {
                if (_photonViewManagerOnPlayer == null) _photonViewManagerOnPlayer = GetComponent<PhotonViewManagerOnPlayer>();
                return _photonViewManagerOnPlayer;
            }
        }
    }
}