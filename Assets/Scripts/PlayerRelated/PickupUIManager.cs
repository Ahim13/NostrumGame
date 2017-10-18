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

        public static PickupUIManager Instance;


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
        [Tooltip("Animation time in seconds on death panel")]
        private float _length;
        [SerializeField]
        [Tooltip("Animation time in seconds ingame")]
        private float _lengthInGame;
        [SerializeField]
        private float _delay;
        [SerializeField]
        [Tooltip("Animation curve for how to roll over the images")]
        private AnimationCurve _animCurve;
        [SerializeField]
        private float _delayToRollAgain;


        public List<Pickups> _pickups;

        private int _randomPickupIndex;
        private Pickups _randomPickup;


        #region Unity Methods

        void Awake()
        {
            SetAsSingleton();

            _pickups = typeof(Pickups)
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(Pickups)) && !t.IsAbstract)
                .Select(t => (Pickups)System.Activator.CreateInstance(t))
                .ToList();

            // _pickups = new List<Pickups>();
            // _pickups.Add((Pickups)System.Activator.CreateInstance(typeof(Revive)));

            foreach (var pickup in _pickups)
            {
                // Debug.Log("Tipus" + pickup.GetType().ToString());
                pickup.PickupSprite = LootManager.Instance.PickupInfos.Where(info => ("NostrumGames." + info.PickupName.ToString()) == pickup.GetType().ToString()).Select(info => info.PickupSprite).First();
            }

        }
        private void SetAsSingleton()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
        }


        #endregion

        public void GetRandomItem()
        {
            _getRandomItemGO.SetActive(false);
            _useItemGO.SetActive(true);
            _useItemGO.GetComponent<Button>().interactable = false;

            //TODO: random chance generator in static class ....
            do
            {
                _randomPickupIndex = Random.Range(0, _pickups.Count);
            }
            while (_pickups[_randomPickupIndex].PickupType != PickupTypes.Offensive && _pickups[_randomPickupIndex].PickupType != PickupTypes.Revive);
            // do
            // {
            //     _randomPickup = LootManager.Instance.GetRandomPickupFromLootTable();
            // }
            // while (_randomPickup.PickupType != PickupTypes.Offensive && _randomPickup.PickupType != PickupTypes.Revive);



            StartCoroutine(RollOverImagesThenShowChosen(_length, _randomPickupIndex, _deadGamePickupImage, true, true, null, null));
        }

        public void RollImagesInGame(System.Type pickedItemType, PickupChooser pickupChooser)
        {
            var itemIndex = _pickups.FindIndex(item => item.GetType() == pickedItemType);
            StartCoroutine(RollOverImagesThenShowChosen(_lengthInGame, itemIndex, _aliveGamePickupImage, false, false, pickupChooser, pickedItemType));
        }
        /// <summary>
        /// Rolls over images then shows the chosen Image
        /// </summary>
        /// <param name="length">The length in seconds</param>
        /// <returns></returns>
        IEnumerator RollOverImagesThenShowChosen(float length, int chosenIndex, Image pickupImage, bool onDeadUI, bool deaccelerate, PickupChooser pickupChooser, System.Type pickedItemType)
        {
            var countDown = length;
            var spriteIndex = 0;
            var timer = 0.0f;
            var lerpedDelay = 0.0f;
            var delayFrom = 0.0f;

            while (countDown >= 0)
            {

                pickupImage.sprite = _pickups[spriteIndex].PickupSprite;

                spriteIndex++;

                if (spriteIndex > _pickups.Count - 1) spriteIndex = 0;

                if (deaccelerate)
                {
                    lerpedDelay = Mathf.Lerp(delayFrom, _delay, _animCurve.Evaluate(timer / _length));
                    timer += Time.smoothDeltaTime + lerpedDelay;
                }


                countDown -= Time.smoothDeltaTime + lerpedDelay;

                yield return new WaitForSecondsRealtime(lerpedDelay);
            }

            pickupImage.sprite = _pickups[chosenIndex].PickupSprite;

            if (onDeadUI) _useItemGO.GetComponent<Button>().interactable = true;
            else pickupChooser.AddPickupComponent(pickedItemType);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                LootManager.Instance.PickupInfos.Clear();
            }
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
            SetImagesToTransparent();
            _getRandomItemGO.SetActive(true);
            _itemImageGO.SetActive(true);
        }

        public void SetImagesToTransparent()
        {
            StopAllCoroutines();
            _deadGamePickupImage.sprite = _transparent;
            _aliveGamePickupImage.sprite = _transparent;
        }



    }
}