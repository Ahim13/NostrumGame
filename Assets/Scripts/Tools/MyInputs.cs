using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace NostrumGames
{
    public class MyInputs : Singleton<MyInputs>
    {
        protected MyInputs() { }
        public IObservable<bool> MoveUp { get; private set; }
        public IObservable<Unit> ActivatePickup { get; private set; }


        void Awake()
        {
            this.Reload();

            var moveUpLatch = this.UpdateAsObservable()
                .Where(_ => Input.GetKey(KeyCode.Space));

            MoveUp = CustomObservables.Latch(this.FixedUpdateAsObservable(), moveUpLatch, false);

            ActivatePickup = this.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.Q));

        }

        // Use this for initialization
        void Start()
        {

        }


    }
}
