using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBoxSpawner : MonoBehaviour
{
    public Vector2 Vel;

    public LayerMask TargetLayerMask;
    public float RayLength;

    private Vector2 _velocity;

    [Header("Spawning")]
    public GameObject SpawnableObject;
    public float TimePerSpawn;

    void Start()
    {
        _velocity = Vel;
    }
    void Update()
    {
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, RayLength, TargetLayerMask);
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, RayLength, TargetLayerMask);

        Debug.DrawRay(transform.position, Vector2.up * RayLength, Color.red);
        Debug.DrawRay(transform.position, Vector2.down * RayLength, Color.blue);

        if (hitUp.collider != null && _velocity.y > 0)
        {
            _velocity = new Vector2(Vel.x, -Vel.y);
        }
        if (hitDown.collider != null && _velocity.y < 0)
        {
            _velocity = new Vector2(Vel.x, Vel.y);
        }
    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = _velocity;
    }
}
