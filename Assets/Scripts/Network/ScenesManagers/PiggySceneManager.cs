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

        public int AngleToAllowRocketSpawn = 5;


        private TerrainRuleGenerator _terrainRuleGenerator;
        private int _currentRuleIndex = -1;
        private bool _shouldPause = false;

        [Header("Obstcale settings")]
        public List<LootTime> SpawnTimes;

        private LootTimeTable _lootGeneric;
        private float _time;
        private float _spawnTime;
        private System.Random _random;

        void Awake()
        {
            SetAsSingleton();
            _random = new System.Random(RandomSeed.MapSeed);

            SceneManager.sceneLoaded += OnSceneFinishedLoading;

            _terrainRuleGenerator = new TerrainRuleGenerator();

            AllowRocketSpawn = false;

            _lootGeneric = new LootTimeTable(SpawnTimes);

            SetRandomTime();
            _time = 0;

        }

        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.P)) Time.timeScale = Time.timeScale == Global.NormalTimeScale ? Global.PausedTimeScale : Global.NormalTimeScale;
            if (Input.GetKeyDown(KeyCode.A))
            {
                // Debug.LogError(RandomSeed.MapSeed);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Paused(!_shouldPause);
                PiggySceneUIManager.Instance.ShouldPause(_shouldPause);
            };

            CheckCurrentTerraintRuleIndex();
            CheckCurrentTerraintRule();
            SpawnObstacle();
        }

        private void SpawnObstacle()
        {
            //Counts up
            _time += Time.deltaTime;

            //Check if its the right time to spawn the object
            if (_time >= _spawnTime)
            {
                SpawnObject();
                SetRandomTime();
            }
        }
        void SpawnObject()
        {
            _time = 0;
            ObstacleSpawner.Instance.SpawnObstcle();
        }

        //Sets the random time between minTime and maxTime
        void SetRandomTime()
        {
            var randRange = _lootGeneric.GetRandomRange();
            _spawnTime = _random.NextFloat(randRange.x, randRange.y);
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
            var currentRuleAnlge = TerrainDisplayer.Instance.TerrainManager.VertexGen.GetPreviousTerrainRuleAngle();

            if (Mathf.Abs(currentRuleAnlge) < AngleToAllowRocketSpawn) AllowRocketSpawn = true;
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

                    PhotonNetwork.Instantiate("Prefabs/Player/Player", new Vector2(6, 7), Quaternion.identity, 0);
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