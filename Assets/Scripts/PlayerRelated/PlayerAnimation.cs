﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{


    public class PlayerAnimation : PlayerBase
    {

        public List<PolygonCollider2D> Colliders { get { return _colliders; } }

        [SerializeField]
        private List<PolygonCollider2D> _colliders;
        private int currentColliderIndex = 0;

        private Rigidbody2D _rigidbody2D;
        private Animator _animator;

        #region Unity Methods

        void Start()
        {
            _animator = GetComponent<Animator>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
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
    }
}