using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{
    public static class RandomSeed
    {
        private static int _seed;
        public static int MapSeed { get; private set; }
        public static void InitRandomSeed()
        {
            _seed = SeedGenerator.GenerateSeed();
            MapSeed = SeedGenerator.GenerateSeed();
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

        public static void SetMapSeed(int newSeed)
        {
            MapSeed = newSeed;
        }
    }
}

public static class SystemRandomExtension
{
    public static float NextFloat(this System.Random random, double minimum, double maximum)
    {
        var randomDouble = random.NextDouble() * (maximum - minimum) + minimum;
        return (float)randomDouble;
    }
}