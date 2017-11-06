using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class JumpingEffect : MonoBehaviour
{


    public Vector2 JumpInPos;
    public Vector2 JumpOutPos;
    public float JumpPower;
    public int NumberOfJumps;
    public float Duration;
    public bool IsUnscaledTime;

    public Ease InEase;
    public Ease OutEase;


    private RectTransform _rectTransform;
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        //JumpIn();
    }

    public void JumpIn()
    {

        _rectTransform.DOJumpAnchorPos(JumpInPos, JumpPower, NumberOfJumps, Duration, false).OnComplete(() => JumpOut()).SetEase(InEase).SetUpdate(IsUnscaledTime);

    }

    public void JumpOut()
    {

        _rectTransform.DOJumpAnchorPos(JumpOutPos, JumpPower, NumberOfJumps, Duration, false).SetEase(OutEase).OnComplete(() => Destroy(this.gameObject)).SetUpdate(IsUnscaledTime);
    }

}