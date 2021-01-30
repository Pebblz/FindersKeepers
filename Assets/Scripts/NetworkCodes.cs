using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// class that stores the network codes for raising events over the photon network
public static class NetworkCodes
{
    public const byte NetworkSceneChangedEventCode = 1;
    public const byte RandomRoomEventCode = 2;
    public const byte DeleteObjectInDropoffCode = 3;
}