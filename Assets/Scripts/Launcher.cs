using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private GameObject controlPanel;
    [SerializeField]
    private GameObject progressLabel;

    
    bool isConnecting;

    const string gameVersion = "1";

    [SerializeField]
    private byte maxPlayersPerRoom = 4;

    public Text playerName;
    public Text roomCode;

    #region MonoBehaviour CallBacks

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }
    #endregion


    #region public methods


    /// <summary>
    /// Connects us to a room hosted by another player, or makes us the host
    /// </summary>
    /// <param name="roomName">name of the room to connect to</param>
    public void Connect()
    {
        string roomName = roomCode.text;

        if (string.IsNullOrEmpty(roomName))
        {
            Debug.LogError("Room name was not provided");
            return;
        }
        progressLabel.SetActive(true);
        controlPanel.SetActive(false);

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRoom(roomName);
        }
        else
        {
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }
    #endregion

    #region PhotonCallbacks

    /// <summary>
    /// if no one else is in the room, create a new room
    /// </summary>
    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            string roomName = roomCode.text;
            PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions { MaxPlayers = maxPlayersPerRoom }, TypedLobby.Default);
            isConnecting = false;
        }
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
        Debug.LogWarningFormat("Disconnected: {0}", cause);
    }

    
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to Join Random Room");
        string roomName = roomCode.text;
        Debug.Log("Creating room " + roomName);
        PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("Loading the Lobby");


            PhotonNetwork.LoadLevel("Lobby_1");
        }
    }

    #endregion

}
