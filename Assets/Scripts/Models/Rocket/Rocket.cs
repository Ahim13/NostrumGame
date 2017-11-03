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


        void Update()
        {
            transform.Translate(DirectionToMove * Time.deltaTime, Space.Self);

            if (Time.frameCount % 10 != 0) return;
            if (Camera.main.WorldToViewportPoint(transform.position).x < DistanceOnXAllowedFromViewport)
            {
                this.gameObject.Despawn();
            }
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                Instantiate(ExplosionParticle, transform.position, transform.rotation);
                this.gameObject.Despawn();
            }
        }

        #endregion
    }
}