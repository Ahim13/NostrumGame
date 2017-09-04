using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;

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