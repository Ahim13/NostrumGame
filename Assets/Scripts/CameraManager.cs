using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{
    public class CameraManager : MonoBehaviour
    {

        [SerializeField]
        private bool useManualSpeed;

        [SerializeField]
        private Vector2 cameraSpeed;
        private Vector2 playerSpeed;
        [SerializeField]
        private Transform player;


        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            useManualSpeed = false;
            playerSpeed = new Vector2(PlayerManager.Instance.SpeedOnXAxis, 0);
        }


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (useManualSpeed) this.transform.Translate(cameraSpeed * Time.deltaTime);
            else this.transform.Translate(playerSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            transform.position = new Vector3(transform.position.x, player.position.y, transform.position.z);
        }

    }
}
