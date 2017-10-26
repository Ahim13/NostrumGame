using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{

    public Rigidbody2D Rigidbody2D;
    public Animator Animator;

    #region Unity Methods

    void Start()
    {

    }


    void Update()
    {
        if (Rigidbody2D.velocity.y < 0) Animator.SetBool("Falling", true);
        else Animator.SetBool("Falling", false);

        if (Input.GetKeyDown(KeyCode.W))
        {
            Rigidbody2D.velocity = new Vector2(0, 1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Rigidbody2D.velocity = new Vector2(0, -1);
        }
    }

    #endregion
}
