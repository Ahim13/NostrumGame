using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{

    public static class PoolExtension
    {

        public static void ResetGameObject(this GameObject gameObject)
        {
            gameObject.SetActive(false);
            gameObject.transform.position = new Vector3(0, 0, 0);
            gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        public static void Spawn(this GameObject gameObject, Vector3 newPos, Quaternion newRot)
        {
            gameObject.SetActive(true);
            gameObject.transform.position = newPos;
            gameObject.transform.rotation = newRot;
        }

        public static void Despawn(this GameObject gameObject)
        {
            gameObject.gameObject.ResetGameObject();

            try
            {
                var pooledObject = gameObject.GetComponent<PoolMember>();
                pooledObject.PoolManager.ListGameObjects.Push(gameObject);

            }
            catch (System.Exception e)
            {
                GameObject.Destroy(gameObject);
                Debug.LogError("Not PoolMono object " + e.Message);
            }
        }

    }
}