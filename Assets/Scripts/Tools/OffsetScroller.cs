using System.Collections;
using System.Collections.Generic;
using NostrumGames;
using UnityEngine;


public class OffsetScroller : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;
    [SerializeField] private bool useRelativeSpeed;
    [SerializeField] [Range(0, 1)] private float relativeSpeedToCharacter;

    private Vector2 savedOffset;
    private float width;

    void Start()
    {
        savedOffset = GetComponent<Renderer>().sharedMaterial.GetTextureOffset("_MainTex");
        width = GetComponent<Renderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float x;

        if (useRelativeSpeed)
        {
            x = GetComponent<Renderer>().sharedMaterial.GetTextureOffset("_MainTex").x +
                Global.PlayersSpeed * Time.fixedDeltaTime / width * relativeSpeedToCharacter;
        }
        else
        {
            x = Time.time * scrollSpeed;
        }

        x = Mathf.Repeat(x, 1);
        Vector2 offset = new Vector2(x, savedOffset.y);
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);
    }

    void OnDisable()
    {
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", savedOffset);
    }
}