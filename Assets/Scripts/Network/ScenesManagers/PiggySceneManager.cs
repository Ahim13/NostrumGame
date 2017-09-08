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

        void Awake()
        {

            SceneManager.sceneLoaded += OnSceneFinishedLoading;

            Time.timeScale = Global.PausedTimeScale;

            StartCoroutine(StartGameAfterSeconds(Scenes.WaitTimeToStart));
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P)) Time.timeScale = Time.timeScale == Global.NormalTimeScale ? Global.PausedTimeScale : Global.NormalTimeScale;
            if (Input.GetKeyDown(KeyCode.A)) Debug.Log(Global.PlayersSpeed);
        }

        private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == Scenes.PiggySceneName)
            {
                PhotonNetwork.Instantiate("Prefabs/Player", new Vector2(5, 6), Quaternion.identity, 0);
            }
        }

        private IEnumerator StartGameAfterSeconds(int sec)
        {
            yield return new WaitForSecondsRealtime(sec);
            Time.timeScale = Global.NormalTimeScale;

        }

    }
}