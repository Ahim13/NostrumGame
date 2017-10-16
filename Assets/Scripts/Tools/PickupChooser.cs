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
            GiveBlur,
            Darken,
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
                    if (col.GetComponent<PlayerManager>().PickupList.Count == 0) AddPickupCompononent(col, col.GetComponent<PlayerManager>());
                    else DestroyBox();

                })
                .AddTo(this);

        }

        private void DestroyBox()
        {
            Destroy(this.gameObject);
        }

        private void AddPickupCompononent(Collider2D col, PlayerManager playerManager)
        {
            Component component = null;
            _randomPickup = Pickups.Shield;
            switch (_randomPickup)
            {
                case Pickups.Darken:
                    component = col.gameObject.AddComponent<Darken>();
                    PickupUIManager.Instance.RollImagesInGame(component.GetComponent<Darken>());
                    break;
                case Pickups.GiveBlur:
                    component = col.gameObject.AddComponent<Confuse>();
                    PickupUIManager.Instance.RollImagesInGame(component.GetComponent<Confuse>());
                    break;
                case Pickups.Shield:
                    component = col.gameObject.AddComponent<Shield>();
                    PickupUIManager.Instance.RollImagesInGame(component.GetComponent<Shield>());
                    break;
                default:
                    break;
            }



            playerManager.PickupList.Add(component);
            DestroyBox();
        }
    }
}
