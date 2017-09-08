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


        private float _lives;
        private ReactiveProperty<float> CurrentLives { get; set; }
        private ReactiveProperty<bool> IsLifeRameined { get; set; }
        private float _currrentLives;

        private bool _noMoreLives;

        private Image _livesImage;

        private PlayerManager _playerManager;


        public LivesCounter(int lives, PlayerManager playerManager)
        {
            this._lives = lives;
            this._livesImage = GameObject.Find(LivesImageName).GetComponent<Image>();

            this.CurrentLives = new ReactiveProperty<float>(lives);
            this.IsLifeRameined = CurrentLives.Select(x => x > 0).ToReactiveProperty();

            CurrentLives.Where(x => x == 0).Subscribe(_ => NoMoreLives());

            this._playerManager = playerManager;
        }

        public void Died()
        {
            _playerManager.IsLiving = false;
            CurrentLives.Value -= 1;
            _livesImage.fillAmount = GetFillAmount(_lives, CurrentLives.Value);
        }


        private float GetFillAmount(float lives, float currrentLives)
        {
            return currrentLives / lives;
        }

        private void NoMoreLives()
        {

            //TODO: for testing
            Debug.Break();
        }
    }

}
