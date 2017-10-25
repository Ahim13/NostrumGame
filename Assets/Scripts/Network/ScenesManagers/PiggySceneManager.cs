using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using UnityEngine.SceneManagement;
using Endless2DTerrain;
using TMPro;

namespace NostrumGames
{

    public class PiggySceneManager : MonoBehaviour
    {

        public static PiggySceneManager Instance;


        public static bool AllowRocketSpawn;


        private TerrainRuleGenerator _terrainRuleGenerator;
        private int _currentRuleIndex = -1;
        private bool _shouldPause = false;

        void Awake()
        {
            SetAsSingleton();

            SceneManager.sceneLoaded += OnSceneFinishedLoading;

            _terrainRuleGenerator = new TerrainRuleGenerator();

            AllowRocketSpawn = false;

            //TODO: do it in final version
            //Time.timeScale = Global.PausedTimeScale;
        }

        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.P)) Time.timeScale = Time.timeScale == Global.NormalTimeScale ? Global.PausedTimeScale : Global.NormalTimeScale;
            if (Input.GetKeyDown(KeyCode.A))
            {
                //StartCountBack();
                //Debug.Log(LootManager.Instance.GetRandomPickupFromLootTable().GetType());
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Paused(!_shouldPause);
                PiggySceneUIManager.Instance.ShouldPause(_shouldPause);
            };

            CheckCurrentTerraintRuleIndex();
            CheckCurrentTerraintRule();
        }

        public void Paused(bool paused)
        {
            _shouldPause = paused;
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
        private void CheckCurrentTerraintRule()
        {
            var currentRule = TerrainDisplayer.Instance.TerrainManager.VertexGen.CurrentTerrainRule;

            if (Mathf.Abs(currentRule.Angle) < 40) AllowRocketSpawn = true;
            else AllowRocketSpawn = false;
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

        public static void StartGame()
        {
            ScoreManager.Instance.IsScoring = true;
            Time.timeScale = Global.NormalTimeScale;
        }

        private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
        {

            try
            {
                if (scene.name == Scenes.PiggySceneName)
                {
                    ApplicationSettings.IsPiggyGameStarted = true;

                    PhotonNetwork.Instantiate("Prefabs/Player", new Vector2(5, 6), Quaternion.identity, 0);
                    if (PhotonPlayerManager.Instance.IsLocalMaster)
                    {
                        PhotonPlayerManager.Reset();
                        PhotonPlayerManager.Instance.MasterLoaded();
                    }
                    else
                    {
                        PhotonPlayerManager.Instance.ClientLoaded();

                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        public void StartCountBack()
        {
            StartCoroutine(PiggySceneUIManager.Instance.StartGameAfterSeconds(Scenes.WaitTimeToStart));
        }

        public void Leave()
        {
            RoomManager.Instance.LeaveRoom();
            SceneManager.LoadScene("Lobby");
        }

    }
}