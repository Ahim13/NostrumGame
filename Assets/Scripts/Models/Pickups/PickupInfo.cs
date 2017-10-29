using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NostrumGames
{
    public enum PickupNames
    {
        Confuse,
        Darken,
        Revive,
        Shield,
        RocketLauncher,
        GainLife,
        EasyMove,
        TnTLauncher
    }

    [System.Serializable]
    public class PickupInfo
    {
        public PickupNames PickupName;
        public Sprite PickupSprite;
        public int Weight;

    }
}