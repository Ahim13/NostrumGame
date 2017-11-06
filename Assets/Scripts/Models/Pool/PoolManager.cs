using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NostrumGames;
using System.Linq;

/*
Pool<GameObject>
 */

public class PoolMember : MonoBehaviour
{
    public PoolManager PoolManager;
}

public class PoolManager
{

    private GameObject _gameObject;

    public Stack<GameObject> ListGameObjects;

    private int _numberOfPools;
    private int _numberOfObjectsInPool;

    private string _containerName;

    private System.Random _random;

    public PoolManager(GameObject gameObject, string containerName, int numberOfObjectsInPool)
    {
        this._gameObject = gameObject;
        this._containerName = containerName;
        this._numberOfObjectsInPool = numberOfObjectsInPool;
        this._random = new System.Random(RandomSeed.MapSeed);

        ListGameObjects = new Stack<GameObject>();

        GeneratePool();
    }

    public void GeneratePool()
    {
        var newContainer = new GameObject(_containerName);

        for (int j = 0; j < _numberOfObjectsInPool; j++)
        {
            var newItem = GameObject.Instantiate(_gameObject, Vector2.zero, Quaternion.identity, newContainer.transform);
            newItem.AddComponent<PoolMember>().PoolManager = this;
            newItem.ResetGameObject();
            ListGameObjects.Push(newItem);
        }

    }
    public void SpawnFromPool(GameObject[] spawnPoints, Vector3 faceTo, int spawnRate)
    {
        if (ListGameObjects.Count >= spawnRate)
        {
            for (int i = 0; i < spawnRate; i++)
            {
                SpawnObject(spawnPoints[i].transform.position, faceTo);
            }
        }
        else
        {
            Debug.Log("No more free item in pool");
        }
    }

    public void SpawnFromPoolByChance(GameObject[] spawnPoints, Vector3 faceTo, int spawnRate, int chanceNotSpawn, int maxNotSpanwed)
    {
        if (ListGameObjects.Count >= spawnRate)
        {
            var skipped = 0;
            var rand = Random.Range(0, 100) + 1;

            var min = ((100 - rand) / spawnRate) + 1;
            var max = (100 - rand);

            for (int i = 0; i < spawnRate; i++)
            {
                if (rand > 100 && skipped == 0)
                {
                    skipped++;
                    continue;
                }

                if (Random.Range(0, 100) < chanceNotSpawn && skipped < maxNotSpanwed)
                {
                    skipped++;
                    continue;
                }
                else
                {
                    rand += Random.Range(min, max);
                }

                SpawnObject(spawnPoints[i].transform.position, faceTo);
            }
        }
        else
        {
            Debug.Log("No more free item in pool");
        }
    }

    public void SpawnFromPoolByChance(GameObject[] spawnPointsMin, GameObject[] spawnPointsMax, Vector3 faceTo, int spawnRate, int chanceNotSpawn, int maxNotSpanwed)
    {
        if (ListGameObjects.Count >= spawnRate)
        {
            var skipped = 0;
            var rand = Random.Range(0, 100) + 1;

            var min = ((100 - rand) / spawnRate) + 1;
            var max = (100 - rand);

            for (int i = 0; i < spawnRate; i++)
            {
                if (rand > 100 && skipped == 0)
                {
                    skipped++;
                    continue;
                }

                if (Random.Range(0, 100) < chanceNotSpawn && skipped < maxNotSpanwed)
                {
                    skipped++;
                    continue;
                }
                else
                {
                    rand += Random.Range(min, max);
                }
                SpawnObject(faceTo, spawnPointsMin[i].transform.position, spawnPointsMax[i].transform.position);
            }
        }
        else
        {
            Debug.Log("No more free item in pool");
        }
    }
    public void SpawnFromPoolOnRandomSpawnPoint(GameObject[] spawnPoints, Vector3 euler)
    {
        if (ListGameObjects.Count != 0)
        {
            SpawnObject(spawnPoints[_random.Next(0, spawnPoints.Length)].transform.position, euler);
        }
        else
        {
            Debug.Log("No more free item in pool");
        }
    }

    private void SpawnObject(Vector3 spawnPoint, Vector3 euler)
    {
        var rocket = ListGameObjects.Pop();
        var newPos = spawnPoint;
        var newRot = Quaternion.Euler(euler);


        rocket.gameObject.Spawn(newPos, newRot);
    }
    private void SpawnObject(Vector3 faceTo, Vector3 minOffset, Vector3 maxOffset)
    {
        var rocket = ListGameObjects.Pop();
        var newPos = GetRandomPositionOnX(minOffset, maxOffset);
        var newRot = Quaternion.FromToRotation(rocket.gameObject.transform.up, faceTo);

        rocket.gameObject.Spawn(newPos, newRot);
    }

    public void PutAllBackToPool()
    {
        // _listGameObjects.GetEnumerator().ForEach(rocket =>
        // {
        //     rocket.gameObject.PutBackToPool();
        // });
    }


    private Vector3 GetRandomPositionOnX(Vector3 min, Vector3 max)
    {
        return new Vector3(Random.Range(min.x, max.x), min.y, min.z);
    }

}
