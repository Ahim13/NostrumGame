using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{
    public class ObstacleSpawner : MonoBehaviour
    {
        public static ObstacleSpawner Instance;

        public GameObject[] SpawnPoints;
        public GameObject SpawnPointsContainer;

        [Header("Obstacle Settings")]
        [SerializeField]
        private GameObject _obstacleToSpawn;
        [SerializeField]
        [Header("Pool settings")]
        private int _numberOfObstacles;


        private PoolManager _poolManager;

        #region Unity Methods

        void Awake()
        {
            SetAsSingleton();
            _poolManager = new PoolManager(_obstacleToSpawn, "ObstacleContainer", _numberOfObstacles);
        }
        private void SetAsSingleton()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
        }

        #endregion

        public void SpawnObstcle()
        {
            var angle = TerrainDisplayer.Instance.TerrainManager.VertexGen.GetPreviousTerrainRuleAngle();
            SpawnPointsContainer.transform.rotation = Quaternion.Euler(0, 0, angle);
            var randomRotation = new Vector3(0, 0, Random.Range(0, 360));
            _poolManager.SpawnFromPoolOnRandomSpawnPoint(SpawnPoints, randomRotation);
        }


#if UNITY_EDITOR
        //Show where the rockets will spawn
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;

            UnityEditor.Handles.Label(SpawnPoints[0].transform.position + new Vector3(0, 1, 0), "SpawnPiont");

            System.Array.ForEach(SpawnPoints, point => Gizmos.DrawSphere(point.transform.position, 0.5f));
        }
#endif
    }
}