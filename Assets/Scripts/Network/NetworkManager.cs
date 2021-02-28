using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // the flag for raising a Network Event

    public void NetworkSceneChangedEvent()
    {
        object[] content = new object[] { };
        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)NetworkCodes.NetworkSceneChanged, content, options, SendOptions.SendReliable);
    }

    public void ChangeToGameMusicEvent()
    {

        object[] content = new object[] { };
        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        bool worked = PhotonNetwork.RaiseEvent((byte)NetworkCodes.ChangeToGameMusic, content, options, SendOptions.SendReliable);
        Debug.Log("Music Event: " + worked);
    }

    public void ChangeToWinOrLoseSceneEvent()
    {
        object[] content = new object[] { };
        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)NetworkCodes.SwitchToWinOrLoseScene, content, options, SendOptions.SendReliable);
    }

    public void ResetToLobbyEvent()
    {
        object[] content = new object[] { };
        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)NetworkCodes.ResetToLobby, content, options, SendOptions.SendReliable);
    }
}
