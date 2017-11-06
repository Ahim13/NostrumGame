using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{


    public class PlayerAnimation : PlayerBase
    {

        public List<PolygonCollider2D> Colliders { get { return _colliders; } }

        [SerializeField]
        private List<PolygonCollider2D> _colliders;
        [SerializeField]
        private ParticleSystem Stars;
        [SerializeField]
        private int EmissionRate = 50;
        private int currentColliderIndex = 0;

        private Rigidbody2D _rigidbody2D;
        private Animator _animator;
        private ParticleSystem.EmissionModule _emission;

        #region Unity Methods

        void Start()
        {
            _animator = GetComponent<Animator>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _emission = Stars.emission;
        }


        void Update()
        {
            if (_rigidbody2D.velocity.y < 0) _animator.SetBool("Falling", true);
            else _animator.SetBool("Falling", false);
        }

        #endregion


        public void SetColliderForSprite(int spriteNum)
        {
            _colliders[currentColliderIndex].enabled = false;
            currentColliderIndex = spriteNum;
            _colliders[currentColliderIndex].enabled = true;
        }

        public void EmitStars(bool emit)
        {
            if (emit) _emission.rateOverTime = EmissionRate;
            else _emission.rateOverTime = 0;
        }
    }
}