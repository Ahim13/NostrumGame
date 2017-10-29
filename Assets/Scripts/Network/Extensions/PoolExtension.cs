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
        public static void TakeFromPool(this GameObject gameObject, bool activate, Vector3 newPos, Quaternion newRot)
        {
            gameObject.SetActive(activate);
            gameObject.transform.position = newPos;
            gameObject.transform.rotation = newRot;
        }

        public static void PutBackToPool(this GameObject gameObject)
        {
            gameObject.ResetGameObject();
            try
            {
                var pooledObject = gameObject.GetComponent<PoolMono>();
                pooledObject.IsInUse = false;

            }
            catch (System.Exception e)
            {

                Debug.LogError("Not PoolMono object " + e.Message);
            }
        }

    }
}