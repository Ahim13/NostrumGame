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

    public PoolManager(GameObject gameObject, string containerName, int numberOfObjectsInPool)
    {
        this._gameObject = gameObject;
        this._containerName = containerName;
        this._numberOfObjectsInPool = numberOfObjectsInPool;

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
    public void SpawnFromPool(Camera camera, Vector3 spawnPoint, Vector3 spawnPointOffset, Vector3 faceTo, int spawnRate)
    {
        if (ListGameObjects.Count >= spawnRate)
        {
            for (int i = 0; i < spawnRate; i++)
            {
                SpawnObject(camera, spawnPoint, spawnPointOffset, faceTo, i);
            }
        }
        else
        {
            Debug.Log("No more free item in pool");
        }
    }

    public void SpawnFromPoolByChance(Camera camera, Vector3 spawnPoint, Vector3 spawnPointOffset, Vector3 faceTo, int spawnRate, int chanceNotSpawn, int maxNotSpanwed)
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

                SpawnObject(camera, spawnPoint, spawnPointOffset, faceTo, i);
            }
        }
        else
        {
            Debug.Log("No more free item in pool");
        }
    }

    public void SpawnFromPoolByChance(Camera camera, Vector3 spawnPoint, Vector3 spawnPointOffset, Vector3 faceTo, int spawnRate, int chanceNotSpawn, int maxNotSpanwed, Vector3 minOffset, Vector3 maxOffset)
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

                SpawnObject(camera, spawnPoint, spawnPointOffset, faceTo, i, minOffset, maxOffset);
            }
        }
        else
        {
            Debug.Log("No more free item in pool");
        }
    }

    private void SpawnObject(Camera camera, Vector3 spawnPoint, Vector3 spawnPointOffset, Vector3 faceTo, int index)
    {
        var rocket = ListGameObjects.Pop();
        var newPos = camera.ViewportToWorldPoint(spawnPoint) + index * spawnPointOffset;
        var newRot = Quaternion.FromToRotation(rocket.gameObject.transform.up, faceTo);

        rocket.gameObject.Spawn(newPos, newRot);
    }
    private void SpawnObject(Camera camera, Vector3 spawnPoint, Vector3 spawnPointOffset, Vector3 faceTo, int index, Vector3 minOffset, Vector3 maxOffset)
    {
        var rocket = ListGameObjects.Pop();
        var newPos = camera.ViewportToWorldPoint(spawnPoint) + index * spawnPointOffset + GetRandomOffsetOnlyX(minOffset, maxOffset);
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


    private Vector3 GetRandomOffsetOnlyX(Vector3 min, Vector3 max)
    {
        return new Vector3(Random.Range(min.x, max.x), 0, 0);
    }

}
