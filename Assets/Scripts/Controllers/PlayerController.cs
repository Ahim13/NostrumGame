using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

namespace NostrumGames
{
    public class PlayerController : MonoBehaviour
    {
        public IObservable<Unit> MovingVelocity { get; private set; }
        public static float GravityScaleDefualt { get; private set; }
        public float VelocityX { get { return velocityX; } private set { } }
        private Rigidbody2D rigidbody2D;
        [SerializeField]
        private float upForce;
        [SerializeField]
        private float gravityScale;
        [SerializeField]
        private float velocityX;
        [SerializeField]
        [Range(0, 1)]
        private float upwardDrag;

        private IDisposable moveUp;
        private IDisposable movingOnX;

        void Awake()
        {
            rigidbody2D = this.GetComponent<Rigidbody2D>();
            GravityScaleDefualt = rigidbody2D.gravityScale;
        }

        void Start()
        {
            MyInputs.Instance.Movement
            .Where(v => v != Vector2.zero)
            .Subscribe(movement =>
            {
                if (rigidbody2D.velocity.y < 0) rigidbody2D.AddForce(movement * 150 * rigidbody2D.mass);
                else
                {
                    rigidbody2D.AddForce(movement * 150 * rigidbody2D.mass);
                }
            })
            .AddTo(this);

            InitBasicMovement();

        }
        private void InitBasicMovement()
        {
            moveUp = MyInputs.Instance.MoveUp
            .Subscribe(pressingSpace =>
            {
                if (pressingSpace)
                {
                    rigidbody2D.AddForce(Vector2.up * (upForce * rigidbody2D.mass));
                }
                else
                {
                    if (rigidbody2D.velocity.y > 0)
                    {
                        var vel = rigidbody2D.velocity;
                        vel.y *= upwardDrag;
                        rigidbody2D.velocity = vel;
                    }
                }
            })
            .AddTo(this);

            MovingOnAxisX();
        }

        private void MovingOnAxisX()
        {
            movingOnX = this.FixedUpdateAsObservable()
                .Subscribe(_ => rigidbody2D.velocity = new Vector2(velocityX, rigidbody2D.velocity.y))
                .AddTo(this);
        }

        public void StartNewLife()
        {
            rigidbody2D.gravityScale = GravityScaleDefualt;
            rigidbody2D.isKinematic = false;
            rigidbody2D.velocity = new Vector2(0, 0);
            InitBasicMovement();
        }

        //TODO: make better "death gravity", now you have to set the gravityScale back to default
        public void KillController()
        {
            moveUp.Dispose();
            movingOnX.Dispose();
            rigidbody2D.velocity = Vector3.zero;
            rigidbody2D.gravityScale = gravityScale;
            // rigidbody2D.isKinematic = true;
        }

        public void IsKinematic(bool kinematic)
        {
            rigidbody2D.isKinematic = kinematic;
        }

    }
}
