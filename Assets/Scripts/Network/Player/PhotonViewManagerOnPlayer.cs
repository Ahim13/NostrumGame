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

    }
}