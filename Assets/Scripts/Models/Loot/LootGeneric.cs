using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;


namespace NostrumGames
{
    [System.Serializable]
    public struct Loot
    {
        [Tooltip("Min and Max")]
        public Vector2 TimeRange;
        [Tooltip("Min and Max")]
        [HideInInspector]
        public Vector2 LootRange;
        public int Weight;
    }

    [System.Serializable]
    public class LootGeneric
    {
        [SerializeField]
        private List<Loot> _loots;
        private int _sumWeights;

        public LootGeneric(List<Loot> loots)
        {
            this._loots = new List<Loot>();
            _sumWeights = loots.Sum(info => info.Weight);
            AssignLootRange(loots);
        }
        private void AssignLootRange(List<Loot> loots)
        {
            var range = 0;
            foreach (var loot in loots)
            {
                var item = new Loot();

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

            var selectedItem = _loots.Where(loot => (IsInRange(loot, randomNumber))).Single();

            return selectedItem.TimeRange;
        }

        /// <summary>
        /// Start is Exclusive End is inclusive
        /// </summary>
        private bool IsInRange(Loot loot, int randomNumber)
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