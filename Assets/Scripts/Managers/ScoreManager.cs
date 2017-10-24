using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NostrumGames
{
    public class ScoreManager : MonoBehaviour
    {

        public static ScoreManager Instance;

        public float Score { get; private set; }
        public bool IsScoring { get; set; }


        [SerializeField]
        private TextMeshProUGUI _textScore;
        [SerializeField]
        private float _scoreIncreasingPerSec;

        [SerializeField]
        private GameObject _playerGO;


        #region Unity Methods

        void Awake()
        {
            SetAsSingleton();

            Score = 0;
            IsScoring = false;
        }
        void Start()
        {
            _playerGO = GameObject.FindGameObjectWithTag("Player");
        }
        private void SetAsSingleton()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
        }


        void Update()
        {
            if (_playerGO != null && _playerGO.GetComponent<PlayerManager>().IsLiving)
            {
                Score += _scoreIncreasingPerSec * Time.deltaTime;

                _textScore.text = Score.ToString("0");
            }
        }

        #endregion
    }
}