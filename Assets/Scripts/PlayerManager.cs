using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace NostrumGames
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerManager : MonoBehaviour
    {
        private PlayerController playerController;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            playerController = this.GetComponent<PlayerController>();
        }

        // Use this for initialization
        void Start()
        {

            this.OnCollisionEnter2DAsObservable()
                .Where(col => col.gameObject.tag == "Map")
                .Subscribe(col =>
                {
                    Debug.Log(col.gameObject.name);
                    playerController.Death();
                })
                .AddTo(this);

        }

        /// <summary>
        /// Sent when an incoming collider makes contact with this object's
        /// collider (2D physics only).
        /// </summary>
        /// <param name="other">The Collision2D data associated with this collision.</param>
        void OnCollisionEnter2D(Collision2D other)
        {

        }
    }
}
