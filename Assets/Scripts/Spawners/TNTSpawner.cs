using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{
    public class TNTSpawner : MonoBehaviour
    {
        public static TNTSpawner Instance;


        [SerializeField]
        private Camera _camera;
        [SerializeField]
        private Vector3 _spawnPoint;
        [SerializeField]
        private Vector3 _spawnPointOffsetMin;
        [SerializeField]
        private Vector3 _spawnPointOffsetMax;


        [Header("TNT Settings")]
        [SerializeField]
        private GameObject _TNTToSpawn;
        [SerializeField]
        private int _numberOfTNT;

        [Tooltip("Out of 100")]
        public int ChanceToNotSpawn = 25;
        [Tooltip("Out of 100")]
        [Range(0, 10)]
        public int MaximumNotSpawnedTNT = 10;



        private List<GameObject> _tnts;

        private LootGeneric _lootGeneric;

        #region Unity Methods

        void Awake()
        {
            SetAsSingleton();
            _tnts = new List<GameObject>();
            GenerateObjects();
        }
        private void SetAsSingleton()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
        }

        void Start()
        {

        }


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.T)) SpawnTNTs();
            if (Input.GetKeyDown(KeyCode.Z)) _tnts.ForEach(tnt => tnt.PutBackToPool());
        }

        #endregion

        public void SpawnTNTs()
        {
            var skipped = 0;
            for (int i = 0; i < _tnts.Count; i++)
            {
                if (Random.Range(0, 100) < ChanceToNotSpawn && skipped < MaximumNotSpawnedTNT)
                {
                    skipped++;
                    continue;
                }

                var newPos = _camera.ViewportToWorldPoint(_spawnPoint) + i * new Vector3(0, _spawnPointOffsetMin.y, 0) + GetRandomOffsetOnlyX(_spawnPointOffsetMin, _spawnPointOffsetMax);
                _tnts[i].TakeFromPool(true, newPos, Quaternion.identity);
            }

            //if 0 was skipped that mean unfair/unplayable
            if (skipped == 0)
            {
                _tnts[_tnts.Count / 2].PutBackToPool();
                Debug.Log("Small chance happened");
            }
        }

        private void GenerateObjects()
        {
            var tntContainer = new GameObject("TNTContainer");
            for (int i = 0; i < _numberOfTNT; i++)
            {
                var newRocket = Instantiate(_TNTToSpawn, Vector2.zero, Quaternion.identity, tntContainer.transform);
                newRocket.ResetGameObject();
                _tnts.Add(newRocket);
            }
        }

        private Vector3 GetRandomOffsetOnlyX(Vector3 min, Vector3 max)
        {
            return new Vector3(Random.Range(min.x, max.x), 0, 0);
        }




#if UNITY_EDITOR
        //Show where the rockets will spawn
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            UnityEditor.Handles.Label(_camera.ViewportToWorldPoint(_spawnPoint) + new Vector3(0, 1, 0), "TNT SpawnPoint");
            //Gizmos.DrawSphere(_camera.ViewportToWorldPoint(_spawnPoint), 0.5f);

            for (int i = 0; i < _numberOfTNT; i++)
            {
                Gizmos.DrawSphere(_camera.ViewportToWorldPoint(_spawnPoint) + i * _spawnPointOffsetMin, 0.4f);
            }
            Gizmos.color = Color.yellow;

            for (int i = 0; i < _numberOfTNT; i++)
            {
                Gizmos.DrawSphere(_camera.ViewportToWorldPoint(_spawnPoint) + i * new Vector3(0, _spawnPointOffsetMax.y, _spawnPointOffsetMax.z) + new Vector3(_spawnPointOffsetMax.x, 0, 0), 0.4f);
            }
        }
#endif
    }
}