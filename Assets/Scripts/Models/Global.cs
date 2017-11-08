using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NostrumGames
{
    public static class Global
    {

        public const string PlayerTagName = "Player";
        public const string OtherPlayerTagName = "OtherPlayer";

        #region Music
        public const string MenuMusic = "MenuMusic";
        public const string GameMusic = "GameMusic";
        public const string Gong = "Gong";
        public const string RocketLaunch = "RocketLaunch";
        public const string CrateBreak = "CrateBreak";
        public const string SlotMachine = "SlotMachine";
        public const string PrizeWinning = "PrizeWinning";
        #endregion

        public static readonly float NormalTimeScale = 1;
        public static readonly float PausedTimeScale = 0;


        public static readonly float DefaultGravity = 0.7f;
        public static readonly float OtherPlayersAlpha = 0.35f;

        public static readonly LayerMask MapMask = LayerMask.NameToLayer("Map");


        public static float PlayersSpeed = 0;

    }
}
