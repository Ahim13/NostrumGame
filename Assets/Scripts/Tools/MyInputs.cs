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
        public IObservable<Vector2> Movement { get; private set; }
        public IObservable<bool> MoveUp { get; private set; }
        public IObservable<Unit> ActivatePickup { get; private set; }


        void Awake()
        {
            Movement = this.FixedUpdateAsObservable()
                .Select(_ =>
                {
                    var x = Input.GetAxis("Horizontal");
                    var y = Input.GetAxis("Vertical");
                    return new Vector2(x, y).normalized;
                });

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
