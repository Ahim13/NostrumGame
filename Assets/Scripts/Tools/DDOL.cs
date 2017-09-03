using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOL : Singleton<DDOL>
{
    void Awake()
    {
        this.Reload();
        DontDestroyOnLoad(this);
    }
}
