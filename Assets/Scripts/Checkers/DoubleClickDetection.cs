using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

namespace NostrumGames
{
    [RequireComponent(typeof(Button))]
    public class DoubleClickDetection : MonoBehaviour
    {

        public UnityEvent OnDoubleClick;

        void Awake()
        {
            var button = this.gameObject.GetComponent<Button>();

            var clicks = button.OnClickAsObservable();

            clicks.Buffer(clicks.Throttle(TimeSpan.FromMilliseconds(200)))
                .Where(xs => xs.Count >= 2)
                .Subscribe(xs => OnDoubleClick.Invoke());
        }
    }
}
