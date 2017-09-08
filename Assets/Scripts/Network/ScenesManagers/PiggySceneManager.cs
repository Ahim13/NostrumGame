using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{

    public class PiggySceneManager : MonoBehaviour
    {

        void Awake()
        {
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P)) Time.timeScale = Time.timeScale == 1 ? 0 : 1;
        }

    }
}