using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{
    public class CameraManager : Singleton<CameraManager>
    {
        protected CameraManager() { }

        public float CameraSpeedAngle { get; set; }

        [SerializeField]
        private bool useManualSpeed = false;

        [SerializeField]
        private Vector3 cameraSpeed;
        private Vector2 playerSpeed;
        [SerializeField]
        private Transform player;


        void Awake()
        {
            playerSpeed = new Vector2(PlayerManager.Instance.SpeedOnXAxis, 0);
        }


        void Start()
        {

        }

        void FixedUpdate()
        {
            if (useManualSpeed) TranslateSpeed(cameraSpeed);
            else TranslateSpeed(playerSpeed);
        }

        void TranslateSpeed(Vector2 speed)
        {
            if (CameraSpeedAngle != 0) this.transform.Translate(Quaternion.Euler(0, 0, CameraSpeedAngle) * speed * Time.deltaTime);
            else this.transform.Translate(speed * Time.deltaTime);
        }



        void Update()
        {

            // var camera = this.GetComponent<Camera>();
            // Vector2 worldPoint = Camera.main.ScreenToWorldPoint(camera.transform.position);
            // RaycastHit2D hit = Physics2D.CircleCast(camera.transform.position, 2, Vector2.zero);
            // if (hit.collider != null)
            // {
            //     Debug.Log(hit.collider.name);
            // }

            //transform.position = new Vector3(transform.position.x, player.position.y, transform.position.z);
        }

    }
}
