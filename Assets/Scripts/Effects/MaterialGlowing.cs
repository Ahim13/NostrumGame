using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MaterialGlowing : MonoBehaviour
{

    public float Duration;
    public Ease EasyType;
    public float GlowFactor;

    private Material _material;


    #region Unity Methods

    void Start()
    {
        _material = GetComponent<Renderer>().material;

        _material.DOColor(new Color(GlowFactor, GlowFactor, GlowFactor, 1), "_EmissionColor", Duration).SetLoops(-1, LoopType.Yoyo).SetEase(EasyType);
    }

    #endregion
}
