using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using NostrumGames;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;

    private Dictionary<string, string> localizedText;
    private bool isReady = false;
    private string missingTextString = "Localized text not found";

    #region Unity Methods

    void Awake()
    {
        SetAsSingleton();
        LoadEnglishLocalization();
    }

    private void SetAsSingleton()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    #endregion

    public void LoadLocalizedText(string fileName)
    {
#if UNITY_WEBGL

        StartCoroutine(GetfilePath(fileName));
#else
        localizedText = new Dictionary<string, string>();
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);

            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

            for (int i = 0; i < loadedData.items.Length; i++)
            {
                localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }
        }
        else
        {
            Debug.LogError("Cannot find file");
        }

        if(isReady) OpeningSceneManager.Instance.SwitchlanguageAndMainPanel();
        isReady = true;
#endif
    }

    private IEnumerator GetfilePath(string fileName)
    {
        localizedText = new Dictionary<string, string>();
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        string result = "";


        UnityWebRequest www = UnityWebRequest.Get(filePath);
        yield return www.SendWebRequest();
        result = www.downloadHandler.text;

        string dataAsJson = result;

        LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

        for (int i = 0; i < loadedData.items.Length; i++)
        {
            localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
        }
        if (isReady) OpeningSceneManager.Instance.SwitchlanguageAndMainPanel();
        isReady = true;

    }

    public string GetLocalizedValue(string key)
    {
        string result = missingTextString;
        if (localizedText.ContainsKey(key))
        {
            result = localizedText[key];
        }

        return result;
    }

    public bool GetIsReady()
    {
        return isReady;
    }

    public void LoadEnglishLocalization()
    {
        LocalizationManager.Instance.LoadLocalizedText("localizedText_en.json");
    }
}
