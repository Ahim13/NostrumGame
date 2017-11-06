using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NostrumGames
{
    public class MySceneManager : MonoBehaviour
    {
        public static MySceneManager Instance;
        void Awake()
        {
            SetAsSingleton();
        }
        private void SetAsSingleton()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
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
        public string GetLoadedSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }
    }
}