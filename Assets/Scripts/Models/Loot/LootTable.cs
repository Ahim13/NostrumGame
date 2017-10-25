using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;

namespace NostrumGames
{
    [System.Serializable]
    struct Item
    {
        public Pickups Pickup;
        public int[] Range;
        public int Weight;
    }

    [System.Serializable]
    public class LootTable
    {
        private List<PickupInfo> _pickupInfos;

        [SerializeField]
        private List<Item> _items;

        private int _sumWeights;

        public LootTable(List<PickupInfo> pickupInfos)
        {
            this._pickupInfos = pickupInfos;

            _items = new List<Item>();

            _sumWeights = this._pickupInfos.Sum(info => info.Weight);

            CreateItemsAndAssignItemRange();

        }

        private void CreateItemsAndAssignItemRange()
        {
            var range = 0;
            foreach (var pickupInfo in _pickupInfos)
            {
                var item = new Item();

                var rangeEnd = range + pickupInfo.Weight;



                var type = System.Type.GetType("NostrumGames." + pickupInfo.PickupName);

                item.Pickup = (Pickups)System.Activator.CreateInstance(type);
                item.Range = new int[2] { range, rangeEnd };
                item.Weight = pickupInfo.Weight;

                range = rangeEnd;

                _items.Add(item);
            }
        }

        public Pickups GetRandomItem()
        {
            var randomNumber = GetRandomNumberInt();

            var selectedItem = _items.Where(item => (IsInRange(item, randomNumber))).Single();

            return selectedItem.Pickup;
        }

        /// <summary>
        /// Start is Exclusive End is inclusive
        /// </summary>
        private bool IsInRange(Item item, int randomNumber)
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