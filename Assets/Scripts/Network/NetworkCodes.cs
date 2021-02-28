using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Fower Box
 * 
 * Edited By: Patrick Naatz
 * changed the NetworkCodes class to an enum
 */
namespace com.pebblz.finderskeepers
{
    public enum NetworkCodes : byte
    {
        NetworkSceneChanged = 1,
        ChangeToGameMusic,
        SwitchToWinOrLoseScene,
        ResetToLobby
    }
}