using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class FlashTextEffect : MonoBehaviour
{
    public TextMeshProUGUI Textmesh;

    public float To;
    public float Duration;
    public Ease EaseType;

    void Start()
    {
        //TargetText.DOFade(To, Duration).SetLoops(-1, LoopType.Yoyo).SetEase(EaseType);
        Textmesh.DOFade(To, Duration).SetLoops(-1, LoopType.Yoyo).SetEase(EaseType).SetUpdate(true);
    }
}
