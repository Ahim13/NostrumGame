using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NostrumGames
{
    public class MySceneManager : Singleton<MySceneManager>
    {
        void Awake()
        {
            this.Reload();
        }
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
        public void LoadScene(int indx)
        {
            SceneManager.LoadScene(indx);
        }
        public void LoadSceneAsync(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
        public void LoadSceneAsync(int indx)
        {
            SceneManager.LoadScene(indx);
        }
    }
}