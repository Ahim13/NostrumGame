using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

namespace NostrumGames
{

    public class PiggySceneManager : MonoBehaviour
    {

        void Awake()
        {
            Time.timeScale = Global.PausedTimeScale;

            StartCoroutine(StartGameAfterSeconds(3));
        }


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P)) Time.timeScale = Time.timeScale == Global.NormalTimeScale ? Global.PausedTimeScale : Global.NormalTimeScale;
            if (Input.GetKeyDown(KeyCode.A)) Debug.Log(Global.PlayersSpeed);
        }

        private IEnumerator StartGameAfterSeconds(int sec)
        {
            yield return new WaitForSecondsRealtime(sec);
            Time.timeScale = Global.NormalTimeScale;

        }

    }
}