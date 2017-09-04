using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using UnityEngine.SceneManagement;

namespace NostrumGames
{

    public class OpeningSceneManager : PunBehaviour
    {
        public static OpeningSceneManager Instance;

        [SerializeField]
        private InputField _playerNameInput;
        [SerializeField]
        private Button _buttonStart;
        [SerializeField]
        private Text _connectingText;
        [SerializeField]
        private Text _badNameWarningText;

        public string LobbySceneName;
        void Awake()
        {
            SetAsSingleton();
            _buttonStart.interactable = false;
            _connectingText.gameObject.SetActive(true);
        }

        private void SetAsSingleton()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
        }

        private bool NameValidation()
        {
            var name = _playerNameInput.text;
            return !string.IsNullOrEmpty(name);
        }

        public void StartGame()
        {
            if (NameValidation())
            {
                MySceneManager.Instance.LoadScene(LobbySceneName);
                LobbyManager.Instance.ConnectToLobby();
                OpeningSceneManager.Instance.SetLocalPlayerName();
            }
            else
            {
                _badNameWarningText.gameObject.SetActive(true);
            }
        }

        public void SetStartButtonInteractable(bool interactable)
        {
            _buttonStart.interactable = interactable;
        }

        public void SetLocalPlayerName()
        {
            PlayerSettings.Instance.SetPlayerName(_playerNameInput.text);
        }

        public override void OnConnectedToMaster()
        {
            SetStartButtonInteractable(true);
            _connectingText.gameObject.SetActive(false);
        }

    }
}