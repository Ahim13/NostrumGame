using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkSceneManager : Singleton<NetworkSceneManager>
{

    private List<string> _sceneNamesInBuild = new List<string>();
    private List<int> _sceneIndexesInBuild = new List<int>();

    void Start()
    {
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            int sceneIndex = SceneUtility.GetBuildIndexByScenePath(scenePath);
            int lastSlash = scenePath.LastIndexOf("/");
            _sceneNamesInBuild.Add(scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1));
            _sceneIndexesInBuild.Add(sceneIndex);
        }
    }

    public void LoadScene(int sceneIndex)
    {
        if (CheckIfSceneAvailable(sceneIndex, null)) PhotonNetwork.LoadLevel(sceneIndex);
        else Debug.LogWarning("SceneiIndex not available");
    }
    public void LoadScene(string sceneName)
    {
        if (CheckIfSceneAvailable(null, sceneName)) PhotonNetwork.LoadLevel(sceneName);
        else Debug.LogWarning("Scene name not available");
    }

    private bool CheckIfSceneAvailable(int? sceneIndex, string sceneName)
    {
        if (sceneIndex != null && SearchForScene((int)sceneIndex)) { return true; }
        if (!string.IsNullOrEmpty(sceneName) && SearchForScene(sceneName)) { return true; }

        return false;
    }

    private bool SearchForScene(int index)
    {
        return _sceneIndexesInBuild.Contains(index);
    }
    private bool SearchForScene(string sceneName)
    {
        return _sceneNamesInBuild.Contains(sceneName);
    }
}
