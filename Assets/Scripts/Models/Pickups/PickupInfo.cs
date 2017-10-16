using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NostrumGames
{
    public enum PickupNames
    {
        Confuse,
        Darken,
        Relive,
        Shield
    }

    [System.Serializable]
    public class PickupInfo
    {
        public PickupNames PickupName;
        public Sprite PickupSprite;

    }
}