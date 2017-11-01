using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;


namespace NostrumGames
{
    [System.Serializable]
    public struct LootTime
    {
        [Tooltip("Min and Max")]
        public Vector2 TimeRange;
        [Tooltip("Min and Max")]
        [HideInInspector]
        public Vector2 LootRange;
        public int Weight;
    }

    [System.Serializable]
    public class LootTimeTable
    {
        [SerializeField]
        private List<LootTime> _loots;
        private int _sumWeights;

        public LootTimeTable(List<LootTime> loots)
        {
            this._loots = new List<LootTime>();
            _sumWeights = loots.Sum(info => info.Weight);
            AssignLootRange(loots);
        }
        private void AssignLootRange(List<LootTime> loots)
        {
            var range = 0;
            foreach (var loot in loots)
            {
                var item = new LootTime();

                var rangeEnd = range + loot.Weight;



                item.TimeRange = loot.TimeRange;
                item.LootRange = new Vector2(range, rangeEnd);
                item.Weight = loot.Weight;

                range = rangeEnd;

                _loots.Add(item);
            }
        }


        public Vector2 GetRandomRange()
        {
            var randomNumber = GetRandomNumberInt();

            var selectedItem = _loots.Where(loot => (IsInRange(loot, randomNumber))).First();

            return selectedItem.TimeRange;
        }

        /// <summary>
        /// Start is Exclusive End is inclusive
        /// </summary>
        private bool IsInRange(LootTime loot, int randomNumber)
        {
            if (loot.LootRange.x < randomNumber && randomNumber <= loot.LootRange.y) return true;
            return false;
        }

        private int GetRandomNumberInt()
        {
            return Random.Range(1, _sumWeights + 1);
        }


    }
}