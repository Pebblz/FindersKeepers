using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// class that stores the network codes for raising events over the photon network
public static class NetworkCodes
{
    public const byte NetworkSceneChangedEventCode = 1;
    public const byte RandomRoomEventCode = 2;
    public const byte DeleteObjectInDropoffCode = 3;
    public const byte ChangeToGameMusicEvent = 4;
    public static string getNameForCode(byte code){
        switch(code){
          case 1:
            return "NetworkSceneChangedEventCode";
          case 2:
            return "RandomRoomEventCode";
          case 3:
            return "DeleteObjectInDropoffCode";
          case 4:
            return "ChangeToGameMusicEvent";
          default:
            return "Unknown Code";
        }
    }
}