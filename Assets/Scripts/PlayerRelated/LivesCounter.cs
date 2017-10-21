using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;
using UniRx;

namespace NostrumGames
{
    public class LivesCounter
    {

        public static readonly string LivesImageName = "LivesImage";



        private float _maxLives;
        private ReactiveProperty<float> CurrentLives { get; set; }
        public ReactiveProperty<bool> HasRemainingLife { get; }

        private float _currrentLives;

        private bool _noMoreLives;

        private Image _livesImage;

        private PlayerManager _playerManager;


        public LivesCounter(int lives, PlayerManager playerManager)
        {
            this._maxLives = lives;
            this._livesImage = GameObject.Find(LivesImageName).GetComponent<Image>();

            this.CurrentLives = new ReactiveProperty<float>(lives);
            this.HasRemainingLife = CurrentLives.Select(x => x > 0).ToReactiveProperty();

            // IsLifeRameined.Where(alive => alive).Subscribe(_ => NoMoreLives());

            CurrentLives.Where(x => x == 0).Subscribe(_ => NoMoreLives());

            this._playerManager = playerManager;
        }

        public void Died()
        {
            _playerManager.IsLiving = false;
            CurrentLives.Value -= 1;
            _livesImage.fillAmount = GetFillAmount(_maxLives, CurrentLives.Value);
        }

        public void Revived()
        {
            _playerManager.IsLiving = true;
            CurrentLives.Value = 1;
            _livesImage.fillAmount = GetFillAmount(_maxLives, CurrentLives.Value);
            PiggySceneUIManager.Instance.SetPanelActivityByAlive(true);
        }


        private float GetFillAmount(float lives, float currrentLives)
        {
            return currrentLives / lives;
        }

        private void NoMoreLives()
        {

            PiggySceneUIManager.Instance.SetPanelActivityByAlive(false);

            //TODO: for testing
            //Debug.Break();
        }
    }

}
