using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{
    public class RocketSpawnManager : MonoBehaviour
    {
        public static RocketSpawnManager Instance;


        [SerializeField]
        private Camera _camera;
        [SerializeField]
        private Vector3 _spawnPoint;
        [SerializeField]
        private Vector3 _spawnPointOffset;


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


        private PoolManager<Rocket> _poolManager;

        #region Unity Methods

        void Awake()
        {
            SetAsSingleton();
            _lootGeneric = new LootTimeTable(SpawnTimes);
            _poolManager = new PoolManager<Rocket>(_rocketToSpawn, "RocketContainer", XTimesRockets * _numberOfRockets);
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
            // if (Input.GetKeyDown(KeyCode.X)) _poolManager.SpawnFromPoolByChance(_camera, _spawnPoint, _spawnPointOffset, Vector3.left, _numberOfRockets, ChanceToNotSpawn, MaximumNotSpawnedRocket);
            // if (Input.GetKeyDown(KeyCode.Y)) _poolManager.PutAllBackToPool();
            // if (Input.GetKeyDown(KeyCode.G)) _poolManager.SpawnFromPool(_camera, _spawnPoint, _spawnPointOffset, Vector3.left, _numberOfRockets);

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
            _poolManager.SpawnFromPoolByChance(_camera, _spawnPoint, _spawnPointOffset, Vector3.left, _numberOfRockets, ChanceToNotSpawn, MaximumNotSpawnedRocket);
        }


        //Spawns the object and resets the time
        void SpawnObject()
        {
            _time = 0;
            _poolManager.SpawnFromPoolByChance(_camera, _spawnPoint, _spawnPointOffset, Vector3.left, _numberOfRockets, ChanceToNotSpawn, MaximumNotSpawnedRocket);
        }

        //Sets the random time between minTime and maxTime
        void SetRandomTime()
        {
            var randRange = _lootGeneric.GetRandomRange();
            _spawnTime = Random.Range(randRange.x, randRange.y);
        }


#if UNITY_EDITOR
        //Show where the rockets will spawn
        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;

            UnityEditor.Handles.Label(_camera.ViewportToWorldPoint(_spawnPoint) + new Vector3(0, 1, 0), "SpawnPiont");
            //Gizmos.DrawSphere(_camera.ViewportToWorldPoint(_spawnPoint), 0.5f);

            for (int i = 0; i < _numberOfRockets; i++)
            {
                Gizmos.DrawSphere(_camera.ViewportToWorldPoint(_spawnPoint) + i * _spawnPointOffset, 0.5f);
            }
        }
#endif
    }
}