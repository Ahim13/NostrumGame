using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{
    public static class Global
    {

        public static readonly float NormalTimeScale = 1;
        public static readonly float PausedTimeScale = 0;


        public static readonly float DefaultGravity = 0.7f;
        public static readonly float OtherPlayersAlpha = 0.35f;

        public static readonly LayerMask MapMask = LayerMask.NameToLayer("Map");

        public static float PlayersSpeed;

    }
}
