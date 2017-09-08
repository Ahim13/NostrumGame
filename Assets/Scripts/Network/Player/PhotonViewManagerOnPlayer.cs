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
            Debug.Log("KREÁLVA lett tőle: " + info.sender.NickName);
            if (info.sender.IsLocal) return;

            SetAlpha(Global.OtherPlayersAlpha);
            RemoveOrDisableAllUnnecessaryComponents();
        }

        public void SetAlpha(float otherPlayersAlpha)
        {
            var tempColor = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(tempColor.r, tempColor.g, tempColor.b, otherPlayersAlpha);
            Debug.Log(tempColor.a);
            Debug.Log(otherPlayersAlpha);
        }

        private void RemoveOrDisableAllUnnecessaryComponents()
        {
            PlayerManager.enabled = false;
            PlayerController.enabled = false;
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<BoxCollider2D>());
        }
    }
}