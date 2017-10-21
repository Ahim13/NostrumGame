using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NostrumGames
{
    public class PiggySceneUIManager : MonoBehaviour
    {

        public static PiggySceneUIManager Instance;


        [SerializeField]
        private GameObject _inGamePanel;
        [SerializeField]
        private GameObject _deathPanel;
        [SerializeField]
        private GameObject _menuPanel;

        [Header("CountBack")]
        [SerializeField]
        private TextMeshProUGUI _textCountBack01;
        [SerializeField]
        private TextMeshProUGUI _textCountBack02;
        [SerializeField]
        private TextMeshProUGUI _textCountBack03;



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
        public void SetPanelActivityByAlive(bool isAlive)
        {
            _inGamePanel.SetActive(isAlive);
            _deathPanel.SetActive(!isAlive);
        }

        public void ShouldPause(bool pasue)
        {
            _inGamePanel.SetActive(!pasue);
            _menuPanel.SetActive(pasue);
        }


        public IEnumerator StartGameAfterSeconds(int sec)
        {

            var jumpingEffect1 = _textCountBack01.GetComponent<JumpingEffect>();
            var jumpingEffect2 = _textCountBack02.GetComponent<JumpingEffect>();
            var jumpingEffect3 = _textCountBack03.GetComponent<JumpingEffect>();

            _textCountBack01.text = "3";

            jumpingEffect1.JumpIn();

            yield return new WaitForSecondsRealtime(1);

            _textCountBack02.text = "2";
            jumpingEffect2.JumpIn();

            yield return new WaitForSecondsRealtime(1);

            _textCountBack03.text = "1";
            jumpingEffect3.JumpIn();

            yield return new WaitForSecondsRealtime(1);

            Time.timeScale = Global.NormalTimeScale;
        }
    }
}