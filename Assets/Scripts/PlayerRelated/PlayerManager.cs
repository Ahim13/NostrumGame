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

        public Component OwnedPickup { get; set; }


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
        [Header("OtherPlayers")]
        [SerializeField]
        private LayerMask _otherPlayersLayermask;
        [SerializeField]
        private string _tag;
        [Header("Effect Icons")]
        public GameObject ConfuseIcon;



        public static GameObject LocalPlayerGO { get; private set; }


        void Awake()
        {
            if (!PhotonViewManagerOnPlayer.IsPhotonViewMine())
            {
                tag = _tag;
                gameObject.layer = LayermaskHelper.LayermaskToLayer(_otherPlayersLayermask);
                return;
            }

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
            MovementUpdate();
        }

        private void MovementUpdate()
        {
            //If its not our charackter then update its position given by the received NetworkedPlayerMovement values
            if (PhotonViewManagerOnPlayer.IsPhotonViewMine()) return;

            PlayerMovement.UpdateNetworkedPostion();
        }


        #region Initialize

        private void InitTools()
        {
            OwnedPickup = null;
            _playerTween = new PlayerTween(transform, this.GetComponent<Renderer>(), MainCamera, PlayerMovement, this);
            _livesCounter = new LivesCounter(_lives, this);
        }
        private void InitVariablesProperties()
        {
            IsLiving = true;
            MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            HasShield = false;
        }
        #endregion
        private void LoseShield()
        {
            if (GetComponent<SpriteOutline>()) _playerTween.OnLoseShield(GetComponent<SpriteOutline>());
        }

        public void DeleteOutlineComponent()
        {
            // Destroy(GetComponent<SpriteOutline>());
            HasShield = false;
            PhotonView.Get(this).RPC("LoseShieldOnline", PhotonTargets.All);
        }

        /// <summary>
        /// Die on collision if no shield, and lose pickup if have any
        /// </summary>
        private void DieOnCollisionWithObstacle()
        {
            _livesCounter.Died();
            _playerTween.OnDeath(_delayAfterDeath);
            PickupUIManager.Instance.SetImagesToTransparent(); //TODO:nem hatékony
            RemovePickup();
        }

        public void RevivePlayer()
        {
            _livesCounter.Revived();
            _playerTween.SetParentingStartReposition();
        }

        public void RemovePickup()
        {
            if (OwnedPickup != null)
            {
                Destroy(OwnedPickup);
                OwnedPickup = null;
            }
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

        public void SetGravAndVelAfterDelay(Vector3 velo, float gravity, float delay)
        {
            DOVirtual.DelayedCall(delay, () => PlayerMovement.SetGravityAndVelocity(Vector3.zero, 0f));
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
            EffectUIManager.Instance.ShowEffectDescription("Shiled granted", 1);
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
            EffectUIManager.Instance.ShowEffectDescription("Darkness has come", 1);
            ShadowEffectManager.Instance.ActivateShadow();

        }
        [PunRPC]
        private void ActivateConfuse()
        {
            EffectUIManager.Instance.ShowEffectDescription("Confused", 1);
            PlayerMovement.ChangeControllerTypeAndGravity(ControllerType.Reflected);
            PlayerMovement.InitBasicMovement();
            ConfuseIcon.SetActive(true);
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
            EffectUIManager.Instance.ShowEffectDescription("Rockets launched", 1);
            RocketSpawnManager.Instance.SpawnRockets();
        }
        [PunRPC]
        private void ActivateTnTLaucher()
        {
            EffectUIManager.Instance.ShowEffectDescription("TnT-s planted", 1);
            TNTSpawner.Instance.SpawnTnTs();
        }

        public void ActivateGainLife()
        {
            EffectUIManager.Instance.ShowEffectDescription("Life added", 1);
            _livesCounter.AddLife();
        }
        public void ActivateEasyMovement()
        {
            EffectUIManager.Instance.ShowEffectDescription("Easy controll", _confuseDuration);
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
