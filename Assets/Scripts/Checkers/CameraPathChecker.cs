using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace NostrumGames
{
    public class CameraPathChecker : MonoBehaviour
    {

        void Awake()
        {
            this.OnTriggerEnter2DAsObservable()
                .Where(col => col.tag == "Path")
                .Subscribe(col =>
                {
                    CameraManager.Instance.TerrainAngle = col.GetComponent<PathPoint>() ? col.GetComponent<PathPoint>().Angle : 0;
                })
                .AddTo(this);


        }
        void Start()
        {

        }
    }
}