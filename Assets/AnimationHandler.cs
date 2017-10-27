using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{

    public Rigidbody2D Rigidbody2D;
    public Animator Animator;
    public ParticleSystem Stars;
    public int EmissionRate;

    private ParticleSystem.EmissionModule _emission;

    #region Unity Methods

    void Start()
    {
        _emission = Stars.emission;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _emission.rateOverTime = EmissionRate;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _emission.rateOverTime = 0;
        }
    }

    #endregion
}
