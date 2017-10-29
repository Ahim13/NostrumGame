using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocalizedText : MonoBehaviour
{

    public string key;

    private bool _initialized = false;

    #region Unity Methods

    void Start()
    {
        var text = GetComponent<TextMeshProUGUI>();
        text.text = LocalizationManager.Instance.GetLocalizedValue(key);
        _initialized = true;
    }

    void OnEnable()
    {
        if (!_initialized) return;
        var text = GetComponent<TextMeshProUGUI>();
        text.text = LocalizationManager.Instance.GetLocalizedValue(key);
    }

    #endregion
}
