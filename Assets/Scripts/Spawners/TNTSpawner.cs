using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{
    public class TNTSpawner : MonoBehaviour
    {
        public static TNTSpawner Instance;


        public GameObject[] SpawnPointsMin;
        public GameObject[] SpawnPointsMax;
        public GameObject SpawnPointsContainer;


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
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                SpawnTnTs();
            }

        }

        #endregion

        public void SpawnTnTs()
        {
            var angle = TerrainDisplayer.Instance.TerrainManager.VertexGen.GetPreviousTerrainRuleAngle();
            SpawnPointsContainer.transform.rotation = Quaternion.Euler(0, 0, angle);
            _poolManager.SpawnFromPoolByChance(SpawnPointsMin, SpawnPointsMax, Vector3.up, _numberOfTNT, ChanceToNotSpawn, MaximumNotSpawnedTNT);
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

            UnityEditor.Handles.Label(SpawnPointsMin[0].transform.position + new Vector3(0, 1, 0), "TNT SpawnPoint");

            System.Array.ForEach(SpawnPointsMin, point => Gizmos.DrawSphere(point.transform.position, 0.4f));
            Gizmos.color = Color.yellow;

            System.Array.ForEach(SpawnPointsMax, point => Gizmos.DrawSphere(point.transform.position, 0.4f));
        }
#endif
    }
}