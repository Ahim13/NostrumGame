using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{
    public class RandomSeed
    {
        private static int _seed;
        public static void SetRandomSeed()
        {
            _seed = SeedGenerator.GenerateSeed();
            Random.InitState(_seed);
        }

        public static void SetSeed(int seed)
        {
            Random.InitState(seed);
        }
        public static void SetSeed(Random.State state)
        {
            Random.state = state;
        }

        public static int GetSeed()
        {
            return _seed;
        }

        public static Random.State GetRandomState()
        {
            return Random.state;
        }
    }
}