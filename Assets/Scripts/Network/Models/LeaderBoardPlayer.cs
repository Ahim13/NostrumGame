using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace NostrumGames
{

    public class LeaderBoardPlayer : MonoBehaviour
    {

        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private TextMeshPro _name;
        [SerializeField]
        private TextMeshPro _score;

        public void ChangeName(string newName)
        {
            _name.text = newName;
        }

        public void ChangeScore(int newScore)
        {
            _score.text = newScore.ToString();
        }
        public void ChangeSprite(Sprite newSprite)
        {
            _spriteRenderer.sprite = newSprite;
        }

    }
}