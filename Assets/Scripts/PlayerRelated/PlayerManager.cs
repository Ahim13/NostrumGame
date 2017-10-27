using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using DG.Tweening;

namespace NostrumGames
{
    public class PlayerManager : PlayerBase, IPunObservable
    {
        protected PlayerManager() { }


        public Camera MainCamera { get; private set; }

        public LivesCounter LivesCounter { get { return _livesCounter; } }

        public bool IsLiving { get; set; }
        public bool HasShield { get; set; }

        public List<Component> PickupList { get; set; }


        private PlayerTween _playerTween;
        private LivesCounter _livesCounter;


        private Vector3 _cameraMiddlePosition;
        private Vector3 _cameraMiddlePositionLatPos;


        [SerializeField]
        private int _lives;

        [Header("Tween Settings")]
        [SerializeField]
        private float _playerStartPointAtX;
        [SerializeField]
        private float _delayAfterDeath;
        [SerializeField]
        private float _timeToReachSpawnpoint;

        [Header("Pickup Settings")]
        [SerializeField]
        private float _confuseDuration = 5;


        public static GameObject LocalPlayerGO { get; private set; }


        void Awake()
        {
            InitVariablesProperties();
            InitTools();
        }


        void Start()
        {

            if (!PhotonViewManagerOnPlayer.IsPhotonViewMine()) return;

            //If it is ours then add to Reference
            LocalPlayerGO = this.gameObject;

            (this).OnCollisionEnter2DAsObservable()
                .Where(_ => IsLiving)
                .Where(col => col.gameObject.tag == "Map" || col.gameObject.tag == "Obstacle")
                .Subscribe(col =>
                {
                    if (!HasShield) DieOnCollisionWithObstacle();
                    else LoseShield();
                })
                .AddTo(this);

            _playerTween.TweenInit(_playerStartPointAtX, _timeToReachSpawnpoint);

        }
        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.A)) ActivateEasyMovement();
            // if (Input.GetKeyDown(KeyCode.S)) Debug.Log(Shield.Value);
        }


        private void InitTools()
        {
            PickupList = new List<Component>();
            _playerTween = new PlayerTween(transform, this.GetComponent<Renderer>(), MainCamera, PlayerMovement, this);
            _livesCounter = new LivesCounter(_lives, this);
        }
        private void InitVariablesProperties()
        {
            IsLiving = true;
            MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            HasShield = false;
        }
        private void LoseShield()
        {
            if (GetComponent<SpriteOutline>()) _playerTween.OnLoseShield(GetComponent<SpriteOutline>());
        }

        public void DeleteOutlineComponent()
        {
            // Destroy(GetComponent<SpriteOutline>());
            GetComponent<Shield>().LoseShield();
        }

        /// <summary>
        /// Die on collision if no shield, and lose pickup if have any
        /// </summary>
        private void DieOnCollisionWithObstacle()
        {
            _livesCounter.Died();
            _playerTween.OnDeath(_delayAfterDeath);
            PickupUIManager.Instance.SetImagesToTransparent(); //TODO:nem hatékony
            if (PickupList.Count > 0) RemovePickup();
        }

        public void RevivePlayer()
        {
            _livesCounter.Revived();
            _playerTween.SetParentingStartReposition();
        }

        public void RemovePickup()
        {
            Destroy(PickupList[0]);
            PickupList.RemoveAt(0);
        }

        public void StartNewLife(Rigidbody2D rigidbody2D)
        {
            rigidbody2D.gravityScale = Global.DefaultGravity;
            rigidbody2D.isKinematic = false;
            rigidbody2D.velocity = new Vector2(0, 0);

            PlayerMovement.IsCollider2DEnabled(true);
            PlayerMovement.ChangeControllerTypeAndGravity(ControllerType.Basic);
            PlayerMovement.InitBasicMovement();
        }

        #region Photon Serialize


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            SerializeState(stream, info);

            PlayerMovement.SerializeState(stream, info);
        }

        private void SerializeState(PhotonStream stream, PhotonMessageInfo info)
        {
            //throw new NotImplementedException();
        }
        #endregion

        #region PunRpc Calls

        #endregion

        #region Pickup Calls

        [PunRPC]
        private void MakeVisibleOnline(int shieldSize)
        {
            this.gameObject.AddComponent<SpriteOutline>().outlineSize = shieldSize;
            Color myColor = new Color();
            ColorUtility.TryParseHtmlString("#33FF00FF", out myColor);
            this.GetComponent<SpriteOutline>().color = myColor;
        }

        [PunRPC]
        private void LoseShieldOnline()
        {
            Destroy(GetComponent<SpriteOutline>());
            Debug.Log("Eltavolit");
        }

        [PunRPC]
        private void ActivateDarken()
        {
            ShadowEffectManager.Instance.ActivateShadow();
        }
        [PunRPC]
        private void ActivateConfuse()
        {
            PlayerMovement.ChangeControllerTypeAndGravity(ControllerType.Reflected);
            PlayerMovement.InitBasicMovement();
            DOVirtual.DelayedCall(_confuseDuration, () =>
                        {
                            if (PlayerMovement.ControllType == ControllerType.Basic) return;  //if player died dont init movement again, not necessary.
                            PlayerMovement.ChangeControllerTypeAndGravity(ControllerType.Basic);
                            PlayerMovement.InitBasicMovement();
                        });
        }
        [PunRPC]
        private void ActivateRocketLaucher()
        {
            RocketSpawnManager.Instance.SpawnRockets();
        }
        public void ActivateGainLife()
        {
            _livesCounter.AddLife();
        }
        public void ActivateEasyMovement()
        {
            PlayerMovement.ChangeControllerTypeAndGravity(ControllerType.ZeroGravity);
            PlayerMovement.InitBasicMovement();
            DOVirtual.DelayedCall(_confuseDuration, () =>
                        {
                            if (PlayerMovement.ControllType == ControllerType.Basic) return;  //if player died dont init movement again, not necessary.
                            PlayerMovement.ChangeControllerTypeAndGravity(ControllerType.Basic);
                            PlayerMovement.InitBasicMovement();
                        });
        }



        #endregion


    }
}
