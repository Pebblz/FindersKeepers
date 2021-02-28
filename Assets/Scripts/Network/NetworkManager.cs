﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // the flag for raising a Network Event

    public void NetworkSceneChangedRaiseEvent()
    {
        object[] content = new object[] { };
        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)NetworkCodes.NetworkSceneChangedEventCode, content, options, SendOptions.SendReliable);
    }

    public void MusicChangeRaiseEvent()
    {

        object[] content = new object[] { };
        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        bool worked = PhotonNetwork.RaiseEvent((byte)NetworkCodes.ChangeToGameMusicEventCode, content, options, SendOptions.SendReliable);
        Debug.Log("Music Event: " + worked);
    }

    public void ChangeToWinOrLoseSceneRaiseEvent()
    {
        object[] content = new object[] { };
        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)NetworkCodes.SwitchToWinOrLoseSceneEventCode, content, options, SendOptions.SendReliable);
    }

    public void ResetToLobbyRaiseEvent()
    {
        object[] content = new object[] { };
        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)NetworkCodes.ResetToLobbyCode, content, options, SendOptions.SendReliable);
    }
}
