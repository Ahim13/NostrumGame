using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSeed : MonoBehaviour
{
    public int Seed;

    void Awake()
    {
        Random.InitState(Seed);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) Time.timeScale = 0;
        if (Input.GetMouseButtonDown(1)) Time.timeScale = 1;
    }
}
