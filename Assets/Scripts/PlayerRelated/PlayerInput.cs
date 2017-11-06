using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace NostrumGames
{
    public class PlayerInput : PlayerBase
    {

        public IObservable<bool> MoveUp { get; private set; }
        public IObservable<float> MoveUpDown { get; private set; }
        public IObservable<Unit> ActivatePickup { get; private set; }


        void Awake()
        {

            var moveUpLatch = this.UpdateAsObservable()
                .Where(_ => Input.GetKey(KeyCode.Space));

            MoveUp = CustomObservables.Latch(this.FixedUpdateAsObservable(), moveUpLatch, false);

            ActivatePickup = this.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.Q));

            MoveUpDown = this.FixedUpdateAsObservable()
                .Select(_ =>
                {
                    var y = Input.GetAxis("Vertical");

                    return y;
                });

        }
    }
}