using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEditor;
using UniRx;

namespace NostrumGames
{
    public class LivesCounter
    {
        private float _lives;
        private ReactiveProperty<float> CurrentLives { get; set; }
        private ReactiveProperty<bool> IsLifeRameined { get; set; }
        private float _currrentLives;

        private bool _noMoreLives;

        private Image _livesImage;

        public LivesCounter(int lives)
        {
            this._lives = lives;
            this._livesImage = GameObject.Find("LivesImage").GetComponent<Image>();

            this.CurrentLives = new ReactiveProperty<float>(lives);
            this.IsLifeRameined = CurrentLives.Select(x => x > 0).ToReactiveProperty();

            CurrentLives.Where(x => x == 0).Subscribe(_ => NoMoreLives());
        }

        public void Died()
        {
            PlayerManager.Instance.IsLiving = false;
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
