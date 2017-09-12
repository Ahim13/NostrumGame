using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using UnityEngine.SceneManagement;

namespace NostrumGames
{

    public class PiggySceneManager : MonoBehaviour
    {

        public static PiggySceneManager Instance;

        void Awake()
        {
            SetAsSingleton();

            SceneManager.sceneLoaded += OnSceneFinishedLoading;

            Time.timeScale = Global.PausedTimeScale;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P)) Time.timeScale = Time.timeScale == Global.NormalTimeScale ? Global.PausedTimeScale : Global.NormalTimeScale;
            if (Input.GetKeyDown(KeyCode.A)) Debug.Log(Global.PlayersSpeed);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Lobby");
                RoomManager.Instance.LeaveRoom();
            };
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneFinishedLoading;
        }

        private void SetAsSingleton()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
        }

        private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == Scenes.PiggySceneName)
            {
                PhotonNetwork.Instantiate("Prefabs/Player", new Vector2(5, 6), Quaternion.identity, 0);
                if (PhotonPlayerManager.Instance.IsLocalMaster)
                {
                    PhotonPlayerManager.Instance.MasterLoaded();
                }
                else
                {
                    PhotonPlayerManager.Instance.ClientLoaded();

                }
            }
        }

        private IEnumerator StartGameAfterSeconds(int sec)
        {
            yield return new WaitForSecondsRealtime(sec);
            Time.timeScale = Global.NormalTimeScale;
        }

        public void StartCountBack()
        {
            StartCoroutine(StartGameAfterSeconds(Scenes.WaitTimeToStart));
        }

    }
}