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

        void Awake()
        {
            this.Reload();

        }
    }
}
