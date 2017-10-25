using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{
    public class RocketSpawnManager : MonoBehaviour
    {
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
        public int MaximumSkippedRocket = 10;

        [Header("Spawn Time Settings")]
        public float TimeBeforeSpawn;
        public float MaxTime = 5;
        public float MinTime = 2;

        private float _time;
        private float _spawnTime;

        private List<GameObject> _rockets;

        #region Unity Methods

        void Awake()
        {
            _rockets = new List<GameObject>();
            GenerateObjects();
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

            //Counts up
            _time += Time.deltaTime;

            //Check if its the right time to spawn the object
            //TODO: check if alloe to spawn! 
            if (_time >= _spawnTime)
            {
                SpawnObject();
                SetRandomTime();
            }

        }


        #endregion

        private void SpawnRockets()
        {
            var skipped = 0;
            for (int i = 0; i < _rockets.Count; i++)
            {
                if (Random.Range(0, 100) < ChanceToNotSpawn && skipped < MaximumSkippedRocket)
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
            _spawnTime = Random.Range(MinTime, MaxTime);
        }




#if UNITY_EDITOR
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