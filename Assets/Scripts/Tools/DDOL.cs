using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOL : MonoBehaviour
{
    public static DDOL Instance;

    void Awake()
    {
        SetAsSingleton();
        DontDestroyOnLoad(this);
    }
    private void SetAsSingleton()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }
}
