using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace NostrumGames
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextReveal : MonoBehaviour
    {

        [SerializeField]
        private float _delay;

        private TextMeshProUGUI _text;

        #region Unity Methods

        void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        void OnEnable()
        {
            StartCoroutine(RevealCharacters());
        }

        void OnDisable()
        {
            StopCoroutine(RevealCharacters());
        }

        #endregion


        IEnumerator RevealCharacters()
        {

            var totalVisibleCharacter = _text.textInfo.characterCount;
            var counter = 0;

            while (counter <= totalVisibleCharacter)
            {
                var visibleCount = counter % (totalVisibleCharacter + 1);

                _text.maxVisibleCharacters = visibleCount;

                counter += 1;

                yield return new WaitForSeconds(_delay);
            }

        }
    }
}