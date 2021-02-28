using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Fower Box
 * 
 * Edited By: Patrick Naatz
 * changed the NetworkCodes class to an enum
 */ 

public enum NetworkCodes : byte
{
    NetworkSceneChangedEventCode = 1,
    ChangeToGameMusicEventCode,
    SwitchToWinOrLoseSceneEventCode,
    ResetToLobbyCode
}