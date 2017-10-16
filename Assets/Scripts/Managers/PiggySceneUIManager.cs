using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiggySceneUIManager : MonoBehaviour
{

    public static PiggySceneUIManager Instance;


    [SerializeField]
    private GameObject _inGamePanel;
    [SerializeField]
    private GameObject _deathPanel;



    #region Unity Methods

    void Awake()
    {
        SetAsSingleton();
    }

    private void SetAsSingleton()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    #endregion

    /// <summary>
    /// Set the proper panel active based on isAlive
    /// </summary>
    /// <param name="isAlive">Is the character alive?</param>
    public void SetPanelActivity(bool isAlive)
    {
        _inGamePanel.SetActive(isAlive);
        _deathPanel.SetActive(!isAlive);
    }
}
