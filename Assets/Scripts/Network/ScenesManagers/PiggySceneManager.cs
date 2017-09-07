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
            if (Input.GetMouseButtonDown(0)) Time.timeScale = 0;
            if (Input.GetMouseButtonDown(1)) Time.timeScale = 1;
        }

    }
}