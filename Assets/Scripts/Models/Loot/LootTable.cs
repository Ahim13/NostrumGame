using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;

namespace NostrumGames
{
    [System.Serializable]
    struct Item<T>
    {
        public T Pickup;
        public int[] Range;
        public int Weight;
    }

    [System.Serializable]
    public class LootTable<T> where T : new()
    {
        private List<PickupInfo> _pickupInfos;

        [SerializeField]
        private List<Item<T>> _items;

        private int _sumWeights;

        public LootTable(List<PickupInfo> pickupInfos)
        {
            this._pickupInfos = pickupInfos;

            _items = new List<Item<T>>();

            _sumWeights = this._pickupInfos.Sum(info => info.Weight);

            CreateItemsAndAssignItemRange();

        }

        private void CreateItemsAndAssignItemRange()
        {
            var range = 0;
            foreach (var pickupInfo in _pickupInfos)
            {
                var item = new Item<T>();

                var rangeEnd = range + pickupInfo.Weight;



                var type = System.Type.GetType("NostrumGames." + pickupInfo.PickupName);

                item.Pickup = (T)System.Activator.CreateInstance(type);
                item.Range = new int[2] { range, rangeEnd };
                item.Weight = pickupInfo.Weight;

                range = rangeEnd;

                _items.Add(item);
            }
        }

        public T GetRandomItem()
        {
            var randomNumber = GetRandomNumberInt();

            var selectedItem = _items.Where(item => (IsInRange(item, randomNumber))).Single();

            return selectedItem.Pickup;
        }

        /// <summary>
        /// Start is Exclusive End is inclusive
        /// </summary>
        private bool IsInRange(Item<T> item, int randomNumber)
        {
            if (item.Range[0] < randomNumber && randomNumber <= item.Range[1]) return true;
            return false;
        }

        public int GetRandomNumberInt()
        {
            return Random.Range(1, _sumWeights + 1);
        }


    }
}