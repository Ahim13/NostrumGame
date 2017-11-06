using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{
    [RequireComponent(typeof(PhotonView))]
    public class PhotonViewManagerOnPlayer : PlayerBase
    {

        private PhotonView _phtotonView;

        void Awake()
        {
            _phtotonView = GetComponent<PhotonView>();
        }

        public bool IsPhotonViewMine()
        {
            return _phtotonView.isMine;
        }

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            // Debug.Log("Created by him: " + info.sender.NickName);
            if (info.sender.IsLocal) return;

            SetAlpha(Global.OtherPlayersAlpha);
            DisableAllUnnecessaryComponents();
        }

        public void SetAlpha(float otherPlayersAlpha)
        {
            var tempColor = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(tempColor.r, tempColor.g, tempColor.b, otherPlayersAlpha);
        }

        private void DisableAllUnnecessaryComponents()
        {
            // PlayerManager.enabled = false;
            // PlayerMovement.enabled = false;
            // PlayerCollision.enabled = false;
            // PlayerInput.enabled = false;
            // GetComponent<Rigidbody2D>().simulated = false;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            // GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}