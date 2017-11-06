using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NostrumGames
{
    public class LootManager : MonoBehaviour
    {
        public static LootManager Instance;

        public List<PickupInfo> PickupInfos { get { return new List<PickupInfo>(_pickupInfos); } } //TODO: might not ok, this only work with value types
        public LootTable LootTable { get { return _lootTable; } }

        [SerializeField]
        private List<PickupInfo> _pickupInfos;

        [SerializeField]
        private LootTable _lootTable;

        #region Unity Methods
        void Awake()
        {
            SetAsSingleton();

            _lootTable = new LootTable(PickupInfos);
        }
        private void SetAsSingleton()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
        }

        void Update()
        {

        }

        #endregion


        public Pickups GetRandomPickupFromLootTable()
        {
            return _lootTable.GetRandomItem();
        }
        public Pickups GetRandomPickupFromLootTableBut(Pickups[] excludedPickups)
        {
            Pickups randomItem;
            do
            {
                randomItem = _lootTable.GetRandomItem();
            }
            while (excludedPickups.Where(exluded => exluded.GetType() == randomItem.GetType()).Any());
            // while (randomItem.GetType() == excludedPickup.GetType());

            return randomItem;
        }
    }
}