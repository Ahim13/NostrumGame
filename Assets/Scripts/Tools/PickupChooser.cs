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

        //TODO: Add every PICKUP
        private enum Pickups
        {
            // GiveBlur,
            // Darken,
            Shield,

        }

        private Pickups _randomPickup;


        static T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(new System.Random().Next(v.Length));
        }

        void Start()
        {
            _randomPickup = RandomEnumValue<Pickups>();


            this.OnTriggerEnter2DAsObservable()
                .Where(col => col.tag == "Player")
                .Subscribe(col =>
                {
                    if (PlayerManager.Instance.PickupList.Count == 0) AddPickupCompononent(col);
                    else DestroyBox();

                })
                .AddTo(this);

        }

        private void DestroyBox()
        {
            Destroy(this.gameObject);
        }

        private void AddPickupCompononent(Collider2D col)
        {
            Component component = null;
            switch (_randomPickup)
            {
                // case Pickups.Darken:
                //     component = col.gameObject.AddComponent<Darken>();
                //     break;
                // case Pickups.GiveBlur:
                //     component = col.gameObject.AddComponent<GiveBlur>();
                //     break;
                case Pickups.Shield:
                    component = col.gameObject.AddComponent<Shield>();
                    break;
                default:
                    break;
            }

            PlayerManager.Instance.PickupList.Add(component);
            Destroy(this.gameObject);
        }
    }
}
