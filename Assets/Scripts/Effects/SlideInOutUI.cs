using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlideInOutUI : MonoBehaviour
{

    public float ReferencPoint;
    public float Duration;
    public Ease Ease;

    private RectTransform _rect;

    #region Unity Methods

    void OnEnable()
    {
        _rect = GetComponent<RectTransform>();

        _rect.DOAnchorPosX(ReferencPoint, Duration, false).SetEase(Ease).From();
    }

    #endregion
}
