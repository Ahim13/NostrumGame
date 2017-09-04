using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeOutTextEffect : MonoBehaviour
{
    public Text TargetText;

    public float From;
    public float Duration;
    public Ease EaseType;


    private void FadeOut()
    {
        TargetText.DOFade(From, Duration).SetEase(EaseType).OnComplete(() => this.gameObject.SetActive(false)).From();
    }

    void OnEnable()
    {
        FadeOut();
    }
}
