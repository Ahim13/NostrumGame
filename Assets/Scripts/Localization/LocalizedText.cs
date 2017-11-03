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
        try
        {
            var text = GetComponent<TextMeshProUGUI>();
            text.text = LocalizationManager.Instance.GetLocalizedValue(key);
            _initialized = true;
        }
        catch (System.Exception e)
        {

            Debug.LogWarning(e.Message + System.Environment.NewLine + e.Data);
            this.enabled = false;
        }
    }

    void OnEnable()
    {
        if (!_initialized) return;
        var text = GetComponent<TextMeshProUGUI>();
        text.text = LocalizationManager.Instance.GetLocalizedValue(key);
    }

    #endregion
}
