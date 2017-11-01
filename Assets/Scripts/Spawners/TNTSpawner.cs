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
        private Vector3 _spawnPointOffset;
        [SerializeField]
        private Vector3 _offsetMaxOnX;


        [Header("TNT Settings")]
        [SerializeField]
        private GameObject _tntToSpawn;
        [SerializeField]
        private int _numberOfTNT;

        [Tooltip("Out of 100")]
        public int ChanceToNotSpawn = 25;
        [Tooltip("Out of 100")]
        [Range(0, 10)]
        public int MaximumNotSpawnedTNT = 10;
        [Header("Pool settings")]
        public int XTimesTnTs;


        private PoolManager _poolManager;

        #region Unity Methods

        void Awake()
        {
            SetAsSingleton();
            _poolManager = new PoolManager(_tntToSpawn, "TnTConstainer", XTimesTnTs * _numberOfTNT);
        }
        private void SetAsSingleton()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
        }

        #endregion

        public void SpawnTnTs()
        {
            _poolManager.SpawnFromPoolByChance(_camera, _spawnPoint, _spawnPointOffset, Vector3.up, _numberOfTNT, ChanceToNotSpawn, MaximumNotSpawnedTNT, _spawnPointOffset, _offsetMaxOnX);
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
                Gizmos.DrawSphere(_camera.ViewportToWorldPoint(_spawnPoint) + i * _spawnPointOffset, 0.4f);
            }
            Gizmos.color = Color.yellow;

            for (int i = 0; i < _numberOfTNT; i++)
            {
                Gizmos.DrawSphere(_camera.ViewportToWorldPoint(_spawnPoint) + i * _spawnPointOffset + new Vector3(_offsetMaxOnX.x, 0, 0), 0.4f);
            }
        }
#endif
    }
}