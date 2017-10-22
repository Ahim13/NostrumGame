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

        private PickupNames _randomPickup;

        private Collider2D _collider;
        private PlayerManager _playerManager;


        static T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(new System.Random().Next(v.Length));
        }

        void Start()
        {
            _randomPickup = RandomEnumValue<PickupNames>();


            this.OnTriggerEnter2DAsObservable()
                .Where(col => col.tag == "Player")
                .Subscribe(col =>
                {
                    if (col.GetComponent<PlayerManager>().PickupList.Count == 0)
                    {
                        _collider = col;
                        _playerManager = _collider.GetComponent<PlayerManager>();
                        ChoosePickupCompononent();
                    }
                    else DestroyBox();

                })
                .AddTo(this);

        }

        private void DestroyBox()
        {
            Destroy(this.gameObject);
            // PhotonNetwork.Destroy(this.gameObject);
        }

        private void ChoosePickupCompononent()
        {
            _randomPickup = PickupNames.Shield;
            switch (_randomPickup)
            {
                case PickupNames.Darken:
                    // component = col.gameObject.AddComponent<Darken>();
                    PickupUIManager.Instance.RollImagesInGame(new Darken(), this);
                    break;
                case PickupNames.Confuse:
                    // component = col.gameObject.AddComponent<Confuse>();
                    PickupUIManager.Instance.RollImagesInGame(new Confuse(), this);
                    break;
                case PickupNames.Shield:
                    // component = col.gameObject.AddComponent<Shield>();
                    PickupUIManager.Instance.RollImagesInGame(new Shield(), this);
                    break;
                default:
                    break;
            }

            // playerManager.PickupList.Add(component);
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
