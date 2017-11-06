using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{
    public class SeedGenerator
    {

        private static Random.State seedGenerator;
        private static int seedGeneratorSeed = 1337;
        private static bool seedGeneratorInitialized = false;
        public static int GenerateSeed()
        {
            // remember old seed
            var temp = Random.state;

            // initialize generator state if needed
            if (!seedGeneratorInitialized)
            {
                Random.InitState(seedGeneratorSeed);
                seedGenerator = Random.state;
                seedGeneratorInitialized = true;
            }

            // set our generator state to the seed generator
            Random.state = seedGenerator;
            // generate our new seed
            var generatedSeed = Random.Range(int.MinValue, int.MaxValue);
            // remember the new generator state
            seedGenerator = Random.state;
            // set the original state back so that normal random generation can continue where it left off
            Random.state = temp;

            return generatedSeed;
        }
    }
}