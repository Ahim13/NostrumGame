using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NostrumGames;
using System.Linq;

/*
Pool<GameObject>
 */

public class Pool<T>
{
    public List<T> ListOfObjects { get; private set; }

    public Pool(List<T> list, bool inUse)
    {
        this.ListOfObjects = list;
    }
}

public class PoolManager<T> where T : MyPooling
{

    private GameObject _gameObject;

    private List<T> _listGameObjects;

    private int _numberOfPools;
    private int _numberOfObjectsInPool;

    private string _containerName;

    public PoolManager(GameObject gameObject, string containerName, int numberOfObjectsInPool)
    {
        this._gameObject = gameObject;
        this._containerName = containerName;
        this._numberOfObjectsInPool = numberOfObjectsInPool;

        _listGameObjects = new List<T>();

        GeneratePool();
    }

    public void GeneratePool()
    {
        var newContainer = new GameObject(_containerName);

        for (int j = 0; j < _numberOfObjectsInPool; j++)
        {
            var newItem = GameObject.Instantiate(_gameObject, Vector2.zero, Quaternion.identity, newContainer.transform);
            newItem.ResetGameObject();
            _listGameObjects.Add(newItem.GetComponent<T>());
        }

    }
    public void SpawnFromPool(Camera camera, Vector3 spawnPoint, Vector3 spawnPointOffset, Vector3 faceTo, int spawnRate)
    {
        if (_listGameObjects.Where(rocket => !rocket.IsInUse).Count() >= spawnRate)
        {
            for (int i = 0; i < spawnRate; i++)
            {
                var rocket = GetAvailableRocket();
                var newPos = camera.ViewportToWorldPoint(spawnPoint) + i * spawnPointOffset;
                var newRot = Quaternion.FromToRotation(rocket.gameObject.transform.up, faceTo);

                rocket.gameObject.TakeFromPool(true, newPos, newRot);
                rocket.IsInUse = true;
            }
        }
        else
        {
            Debug.Log("No more free item in pool");
        }


    }

    private T GetAvailableRocket()
    {
        return _listGameObjects.Where(rocket => !rocket.IsInUse).First();
    }

    private void PutAllBackToPool()
    {

    }


}
