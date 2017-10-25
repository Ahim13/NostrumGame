﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{
    public class LootManager : MonoBehaviour
    {
        public static LootManager Instance;

        public List<PickupInfo> PickupInfos { get { return new List<PickupInfo>(_pickupInfos); } } //TODO: might not ok, this only work with value types
        public LootTable<Pickups> LootTable { get { return _lootTable; } }

        [SerializeField]
        private List<PickupInfo> _pickupInfos;

        private LootTable<Pickups> _lootTable;

        #region Unity Methods
        void Awake()
        {
            SetAsSingleton();

            _lootTable = new LootTable<Pickups>(PickupInfos);
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
    }
}