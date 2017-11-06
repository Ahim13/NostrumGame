using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{

    public class Obstacle : MonoBehaviour
    {


        public float DistanceOnXAllowedFromViewport;

        #region Unity Methods
        void Update()
        {
            if (Time.frameCount % 10 != 0) return;
            if (Camera.main.WorldToViewportPoint(transform.position).x < DistanceOnXAllowedFromViewport)
            {
                this.gameObject.Despawn();
            }
        }

        #endregion
    }
}
