using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroyer : MonoBehaviour
{

    private ParticleSystem _particle;

    #region Unity Methods

    void Start()
    {
        _particle = GetComponent<ParticleSystem>();
    }


    void Update()
    {
        if (_particle)
        {
            if (!_particle.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }

    #endregion
}
