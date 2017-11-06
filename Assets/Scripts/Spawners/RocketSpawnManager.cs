using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{
    public class RocketSpawnManager : MonoBehaviour
    {
        public static RocketSpawnManager Instance;

        public GameObject[] SpawnPoints;
        public GameObject SpawnPointsContainer;

        [Header("Rocket Settings")]
        [SerializeField]
        private GameObject _rocketToSpawn;
        [SerializeField]
        private int _numberOfRockets;

        [Tooltip("Out of 100")]
        public int ChanceToNotSpawn = 25;
        [Tooltip("Out of 100")]
        [Range(0, 10)]
        public int MaximumNotSpawnedRocket = 10;

        [Header("Spawn Time Settings")]
        public float TimeBeforeFirstSpawn = 30;
        public List<LootTime> SpawnTimes;

        [Header("Pool settings")]
        public int XTimesRockets;

        private float _time;
        private float _spawnTime;

        private LootTimeTable _lootGeneric;


        private PoolManager _poolManager;

        #region Unity Methods

        void Awake()
        {
            SetAsSingleton();
            _lootGeneric = new LootTimeTable(SpawnTimes);
            _poolManager = new PoolManager(_rocketToSpawn, "RocketContainer", XTimesRockets * _numberOfRockets);
        }
        private void SetAsSingleton()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
        }

        void Start()
        {
            SetRandomTime();
            _time = 0;
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                SpawnRockets();
            }

        }

        void FixedUpdate()
        {
            if (Time.timeSinceLevelLoad < TimeBeforeFirstSpawn) return;
            if (!PiggySceneManager.AllowRocketSpawn) return;

            //Counts up
            _time += Time.deltaTime;

            //Check if its the right time to spawn the object
            if (_time >= _spawnTime)
            {
                SpawnObject();
                SetRandomTime();
            }

        }

        #endregion

        public void SpawnRockets()
        {
            var angle = TerrainDisplayer.Instance.TerrainManager.VertexGen.GetPreviousTerrainRuleAngle();
            SpawnPointsContainer.transform.rotation = Quaternion.Euler(0, 0, angle);
            _poolManager.SpawnFromPoolByChance(SpawnPoints, new Vector3(0, 0, 90 + angle), _numberOfRockets, ChanceToNotSpawn, MaximumNotSpawnedRocket);
        }


        //Spawns the object and resets the time
        void SpawnObject()
        {
            _time = 0;
            SpawnRockets();
        }

        //Sets the random time between minTime and maxTime
        void SetRandomTime()
        {
            var randRange = _lootGeneric.GetRandomRange();
            _spawnTime = Random.Range(randRange.x, randRange.y);
        }


#if UNITY_EDITOR
        //Show where the rockets will spawn
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;

            UnityEditor.Handles.Label(SpawnPoints[0].transform.position + new Vector3(0, 1, 0), "SpawnPiont");

            System.Array.ForEach(SpawnPoints, point => Gizmos.DrawSphere(point.transform.position, 0.5f));
        }
#endif
    }
}