using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlideInOutUI : MonoBehaviour
{

    public float ReferencPoint;
    public float Duration;
    public Ease Ease;

    public bool OnX = true;
    public bool OnY = false;

    private RectTransform _rect;

    #region Unity Methods

    void OnEnable()
    {
        _rect = GetComponent<RectTransform>();

        if (OnX) _rect.DOAnchorPosX(ReferencPoint, Duration, false).SetEase(Ease).From().SetUpdate(true);
        if (OnY) _rect.DOAnchorPosY(ReferencPoint, Duration, false).SetEase(Ease).From().SetUpdate(true);
    }

    #endregion
}
