using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{

    public class PiggySceneManager : MonoBehaviour
    {
        public int seed;

        void Awake()
        {
            SetRandomSeed();
        }

        public void SetRandomSeed()
        {
            // int seed;
            seed = SeedGenerator.GenerateSeed();
            Random.InitState(seed);
        }

    }
}