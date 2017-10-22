using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;
using TMPro;

namespace NostrumGames
{

    public class OpeningSceneManager : PunBehaviour
    {
        public static OpeningSceneManager Instance;

        [SerializeField]
        private TMP_InputField _playerNameInput;
        [SerializeField]
        private Button _buttonStart;
        [SerializeField]
        private TextMeshProUGUI _connectingText;
        [SerializeField]
        private TextMeshProUGUI _badNameWarningText;

        public string LobbySceneName;
        void Awake()
        {
            SetAsSingleton();
            _buttonStart.interactable = false;
            _connectingText.gameObject.SetActive(true);
            ApplicationSettings.IsStarted = false;

            SetButtonSubscription();
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

        /// <summary>
        /// Load lobby scene
        /// </summary>
        public void StartGame()
        {
            if (NameValidation())
            {
                ApplicationSettings.IsStarted = true;
                LobbyManager.Instance.ConnectToLobby();
                OpeningSceneManager.Instance.SetLocalPlayerName();
                SceneManager.LoadScene(LobbySceneName);
            }
            else
            {
                _badNameWarningText.gameObject.SetActive(true);
                _playerNameInput.Select();
                _playerNameInput.ActivateInputField();
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

        private void SetButtonSubscription()
        {
            this.UpdateAsObservable()
                .Where(_ => (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
                .Subscribe(_ => _buttonStart.onClick.Invoke())
                .AddTo(this);
        }

    }
}