using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{

    public class Rocket : MonoBehaviour
    {

        public Vector2 DirectionToMove;

        public ParticleSystem BurnerParticle;
        public ParticleSystem ExplosionParticle;

        public float DistanceOnXAllowedFromViewport;

        #region Unity Methods

        void Start()
        {

        }


        void Update()
        {
            transform.Translate(DirectionToMove * Time.deltaTime, Space.World);

            if (Camera.main.WorldToViewportPoint(transform.position).x < DistanceOnXAllowedFromViewport) this.gameObject.PutBackToPool();
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