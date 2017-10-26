using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UniRx;

namespace NostrumGames
{
    public class LivesCounter
    {

        public static readonly string LivesImageName = "LivesImage";
        public static readonly string PlusLifeTextName = "PlusLifeText";



        private float _maxLives;
        private ReactiveProperty<float> CurrentLives { get; set; }
        public ReactiveProperty<bool> HasRemainingLife { get; }

        private Image _livesImage;
        private TextMeshProUGUI _plusLifeText;

        private PlayerManager _playerManager;


        public LivesCounter(int lives, PlayerManager playerManager)
        {
            this._maxLives = lives;
            this._livesImage = GameObject.Find(LivesImageName).GetComponent<Image>();
            this._plusLifeText = GameObject.Find(PlusLifeTextName).GetComponent<TextMeshProUGUI>();


            this.CurrentLives = new ReactiveProperty<float>(lives);
            this.HasRemainingLife = CurrentLives.Select(x => x > 0).ToReactiveProperty();

            CurrentLives.Where(x => x == 0).Subscribe(_ => NoMoreLives());

            this._playerManager = playerManager;

            SpecifyLives();

            //init IsAlive property
            //PhotonPlayerManager.Instance.UpdatePlayerProperty(PlayerProperty.IsAlive, true);
        }


        public void AddLife()
        {
            CurrentLives.Value++;
            SpecifyLives();
        }



        public void Died()
        {
            _playerManager.IsLiving = false;
            CurrentLives.Value -= 1;
            SpecifyLives();
        }

        public void Revived()
        {
            _playerManager.IsLiving = true;
            CurrentLives.Value = 1;
            _livesImage.fillAmount = GetFillAmount(_maxLives, CurrentLives.Value);
            PiggySceneUIManager.Instance.SetPanelActivityByAlive(true);

            PhotonNetwork.room.ChangeAlivePlayersInRoomSettings(1);
        }


        private float GetFillAmount(float lives, float currrentLives)
        {
            return currrentLives / lives;
        }

        private void NoMoreLives()
        {
            ShadowEffectManager.Instance.DeactivateShadow();

            PhotonPlayerManager.Instance.LocalPlayer.SetScore((int)ScoreManager.Instance.Score);

            PhotonNetwork.room.ChangeAlivePlayersInRoomSettings(-1);

            PiggySceneUIManager.Instance.SetPanelActivityByAlive(false);

        }

        private void SpecifyLives()
        {
            if (CurrentLives.Value > _maxLives) ShowText("+" + (CurrentLives.Value - _maxLives).ToString());
            else
            {
                ResetText();
                _livesImage.fillAmount = GetFillAmount(_maxLives, CurrentLives.Value);
            }
        }

        private void ResetText()
        {
            _plusLifeText.text = "";
            _plusLifeText.gameObject.SetActive(false);
        }
        private void ShowText(string text)
        {
            _plusLifeText.text = text;
            _plusLifeText.gameObject.SetActive(true);
        }
    }

}
