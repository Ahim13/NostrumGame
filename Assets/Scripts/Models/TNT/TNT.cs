using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{
    public class TNT : MonoBehaviour
    {

        public ParticleSystem ExplosionParticle;

        public float DistanceOnXAllowedFromViewport;

        #region Unity Methods

        void Start()
        {

        }


        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                Debug.Log("Hit");
                // this.enabled = false;
                Instantiate(ExplosionParticle, transform.position, transform.rotation);
                this.gameObject.PutBackToPool();
            }
        }

        #endregion
    }
}

