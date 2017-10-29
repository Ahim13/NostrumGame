using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace NostrumGames
{
    public class PickupChooser : MonoBehaviour
    {

        private Pickups _randomPickup;

        private Collider2D _collider;
        private PlayerManager _playerManager;


        static T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(new System.Random().Next(v.Length));
        }

        void Start()
        {
            // _randomPickup = RandomEnumValue<PickupNames>();
            _randomPickup = LootManager.Instance.GetRandomPickupFromLootTableBut(new Revive());


            this.OnTriggerEnter2DAsObservable()
                .Where(col => col.tag == "Player")
                .Subscribe(col =>
                {
                    var playerManager = col.GetComponent<PlayerManager>();
                    if (playerManager.PickupList.Count == 0 && playerManager.IsLiving)
                    {
                        _collider = col;
                        _playerManager = _collider.GetComponent<PlayerManager>();
                        ChoosePickupCompononent();
                    }
                    else
                    {
                        Debug.Log("DestroyTheBox");
                        DestroyBox();
                    }

                })
                .AddTo(this);

        }

        private void DestroyBox()
        {
            Destroy(this.gameObject);
        }

        private void ChoosePickupCompononent()
        {
            //_randomPickup = new GainLife();

            PickupUIManager.Instance.RollImagesInGame(_randomPickup, this);

            DestroyBox();
        }

        public void AddPickupComponent(Pickups pickupType)
        {
            Component component = null;

            component = _collider.gameObject.AddComponent(pickupType.GetType());

            _playerManager.PickupList.Add(component);

        }
    }
}
