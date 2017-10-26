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
        public float TimeBeforeSpawn;
        public List<Loot> SpawnTimes;

        private float _time;
        private float _spawnTime;

        private List<GameObject> _rockets;

        private LootGeneric _lootGeneric;

        #region Unity Methods

        void Awake()
        {
            SetAsSingleton();
            _rockets = new List<GameObject>();
            _lootGeneric = new LootGeneric(SpawnTimes);
            GenerateObjects();
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
            if (Input.GetKeyDown(KeyCode.X)) SpawnRockets();
            if (Input.GetKeyDown(KeyCode.Y)) _rockets.ForEach(rocket => rocket.PutBackToPool());

        }
        void FixedUpdate()
        {
            if (Time.timeSinceLevelLoad < TimeBeforeSpawn) return;
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
            var skipped = 0;
            for (int i = 0; i < _rockets.Count; i++)
            {
                if (Random.Range(0, 100) < ChanceToNotSpawn && skipped < MaximumNotSpawnedRocket)
                {
                    skipped++;
                    continue;
                }

                var newPos = _camera.ViewportToWorldPoint(_spawnPoint) + i * _spawnPointOffset;
                var newRot = Quaternion.FromToRotation(_rockets[i].transform.up, Vector3.left);
                _rockets[i].TakeFromPool(true, newPos, newRot);
            }

            //if 0 was skipped that mean unfair/unplayable
            if (skipped == 0)
            {
                _rockets[_rockets.Count / 2].PutBackToPool();
                Debug.Log("Small chance happened");
            }
        }

        private void GenerateObjects()
        {
            var rocketContainer = new GameObject("RocketContainer");
            for (int i = 0; i < _numberOfRockets; i++)
            {
                var newRocket = Instantiate(_rocketToSpawn, Vector2.zero, Quaternion.identity, rocketContainer.transform);
                newRocket.ResetGameObject();
                _rockets.Add(newRocket);
            }
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
            Gizmos.color = Color.red;

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