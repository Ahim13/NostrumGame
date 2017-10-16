using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace NostrumGames
{
    public class PickupUIManager : MonoBehaviour
    {


        [Space(10)]
        [SerializeField]
        private Image _aliveGamePickupImage;
        [SerializeField]
        private Image _deadGamePickupImage;

        [SerializeField]
        private GameObject _getRandomItemGO;
        [SerializeField]
        private GameObject _useItemGO;
        [SerializeField]
        private GameObject _itemImageGO;

        [Header("Defaults")]
        [SerializeField]
        private Sprite _transparent;

        [Header("Values")]
        [SerializeField]
        [Tooltip("In seconds")]
        private float _length;
        [SerializeField]
        private float _delay;
        [SerializeField]
        private bool _deaccelerate;
        [SerializeField]
        private AnimationCurve _animCurve;
        [SerializeField]
        private float _delayToRollAgain;


        private List<Pickups> _pickups;

        private int _randomPickupIndex;


        #region Unity Methods

        void Awake()
        {
            _pickups = typeof(Pickups)
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(Pickups)) && !t.IsAbstract)
                .Select(t => (Pickups)System.Activator.CreateInstance(t))
                .ToList();

        }


        #endregion

        public void GetRandomItem()
        {
            StartCoroutine(GetRandomItem_Coroutine());
            _getRandomItemGO.SetActive(false);
            _useItemGO.SetActive(true);
            _useItemGO.GetComponent<Button>().interactable = false;

        }

        IEnumerator GetRandomItem_Coroutine()
        {
            var countDown = _length;
            var spriteIndex = 0;
            var timer = 0.0f;
            var lerpedDelay = 0.0f;
            var delayFrom = 0.0f;

            if (!_deaccelerate) lerpedDelay = _delay;

            //TODO: random chance generator in static class ....
            do
            {
                _randomPickupIndex = Random.Range(0, _pickups.Count);
            }
            while (_pickups[_randomPickupIndex].PickupType != PickupTypes.Offensive && _pickups[_randomPickupIndex].PickupType != PickupTypes.Relive);


            while (countDown >= 0)
            {

                _deadGamePickupImage.sprite = _pickups[spriteIndex].PickupSprite;

                spriteIndex++;

                if (spriteIndex > _pickups.Count - 1) spriteIndex = 0;

                if (_deaccelerate)
                {
                    lerpedDelay = Mathf.Lerp(delayFrom, _delay, _animCurve.Evaluate(timer / _length));
                    timer += Time.smoothDeltaTime + lerpedDelay;
                }


                countDown -= Time.smoothDeltaTime + lerpedDelay;

                yield return new WaitForSecondsRealtime(lerpedDelay);
            }

            _deadGamePickupImage.sprite = _pickups[_randomPickupIndex].PickupSprite;

            _useItemGO.GetComponent<Button>().interactable = true;
        }

        public void UseItem()
        {
            //TODO: using item online
            _pickups[_randomPickupIndex].ActivatePickup();

            //after usage turn it off for some time
            _useItemGO.SetActive(false);
            _itemImageGO.SetActive(false);

            DOVirtual.DelayedCall(_delayToRollAgain, EnableRandomItemGetting, true);
        }

        private void EnableRandomItemGetting()
        {
            _deadGamePickupImage.sprite = _transparent;
            _getRandomItemGO.SetActive(true);
            _itemImageGO.SetActive(true);
        }



    }
}