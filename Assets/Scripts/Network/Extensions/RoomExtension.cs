using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace NostrumGames
{

    public static class RoomExtension
    {

        public static void SaveNumberOfPlayersToRoomSettings(this Room room, int numberOfPlayers)
        {
            var customPropHash = new Hashtable();
            customPropHash.Add(RoomProperty.AlivePlayers, numberOfPlayers);

            room.SetCustomProperties(customPropHash);
        }
        public static void ChangeAlivePlayersInRoomSettings(this Room room, int addedNumber)
        {

            var alivePlayers = room.GetAlivePlayerInRoom();
            alivePlayers += addedNumber;

            var customPropHash = new Hashtable();
            customPropHash.Add(RoomProperty.AlivePlayers, alivePlayers);


            room.SetCustomProperties(customPropHash);
        }

        public static void SaveWinnerID(this Room room, int ID)
        {
            var customPropHash = new Hashtable();
            customPropHash.Add(RoomProperty.WinnerID, ID);

            room.SetCustomProperties(customPropHash);
        }

        public static int GetAlivePlayerInRoom(this Room room)
        {
            object score;
            if (room.CustomProperties.TryGetValue(RoomProperty.AlivePlayers, out score))
            {
                return (int)score;
            }
            return 0;
        }
    }
}