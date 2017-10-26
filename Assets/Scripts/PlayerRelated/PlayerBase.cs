using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerManager))]
    [RequireComponent(typeof(PhotonViewManagerOnPlayer))]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerCollision))]
    public class PlayerBase : MonoBehaviour
    {
        PlayerMovement _playerMovement;
        public PlayerMovement PlayerMovement
        {
            get
            {
                if (_playerMovement == null) _playerMovement = GetComponent<PlayerMovement>();
                return _playerMovement;
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

        PlayerInput _playerInput;
        public PlayerInput PlayerInput
        {
            get
            {
                if (_playerInput == null) _playerInput = GetComponent<PlayerInput>();
                return _playerInput;
            }
        }

        PlayerCollision _playerCollision;
        public PlayerCollision PlayerCollision
        {
            get
            {
                if (_playerCollision == null) _playerCollision = GetComponent<PlayerCollision>();
                return _playerCollision;
            }
        }
        PlayerAnimation _playerAnimation;
        public PlayerAnimation PlayerAnimation
        {
            get
            {
                if (_playerAnimation == null) _playerAnimation = GetComponent<PlayerAnimation>();
                return _playerAnimation;
            }
        }
    }
}