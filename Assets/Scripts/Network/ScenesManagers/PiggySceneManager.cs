using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using UnityEngine.SceneManagement;
using Endless2DTerrain;

namespace NostrumGames
{

    public class PiggySceneManager : MonoBehaviour
    {

        public static PiggySceneManager Instance;


        private TerrainRuleGenerator _terrainRuleGenerator;
        private int _currentRuleIndex = -1;

        void Awake()
        {
            SetAsSingleton();

            SceneManager.sceneLoaded += OnSceneFinishedLoading;

            _terrainRuleGenerator = new TerrainRuleGenerator();

            //Time.timeScale = Global.PausedTimeScale;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P)) Time.timeScale = Time.timeScale == Global.NormalTimeScale ? Global.PausedTimeScale : Global.NormalTimeScale;
            if (Input.GetKeyDown(KeyCode.A))
            {

            };
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Lobby");
                RoomManager.Instance.LeaveRoom();
            };

            CheckCurrentTerraintRuleIndex();
        }

        private void CheckCurrentTerraintRuleIndex()
        {
            if (TerrainDisplayer.Instance.TerrainManager.VertexGen.GetCurrentTerrainRuleIndex() != _currentRuleIndex)
            {
                _currentRuleIndex = TerrainDisplayer.Instance.TerrainManager.VertexGen.GetCurrentTerrainRuleIndex();

                //if it changed then add new rule
                var newRule = _terrainRuleGenerator.GenerateRandomRule(TerrainRule.TerrainLength.Fixed, -1, 1, 1, 4, 5);
                _terrainRuleGenerator.AddToTerrainRules(newRule);
            }
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