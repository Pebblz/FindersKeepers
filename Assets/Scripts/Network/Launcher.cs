﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

namespace com.pebblz.finderskeepers
{
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

        public InputField playerName;
        public InputField roomCode;

        #region MonoBehaviour CallBacks

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;

        }

        public void SetTextToUpper(InputField f)
        {
            f.text = f.text.ToUpper();
        }

        private void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            playerName.onValueChanged.AddListener(delegate { SetTextToUpper(playerName); });
            roomCode.onValueChanged.AddListener(delegate { SetTextToUpper(roomCode); });
        }
        private void Update()
        {
            //in unity return means enter for some reason
            if (Input.GetKeyDown(KeyCode.Return) && roomCode.text != "" ||
                 Input.GetKeyDown(KeyCode.KeypadEnter) && roomCode.text != "")
            {
                Connect();
            }
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


            if (string.IsNullOrEmpty(playerName.text))
            {
                Debug.LogError("Name was not provided");
                return;
            }

            if (string.IsNullOrEmpty(roomName))
            {
                Debug.LogError("Room code was not provided");
                return;
            }

            PhotonNetwork.NickName = playerName.text.ToUpper();
            Debug.Log("Nickname set to " + PhotonNetwork.LocalPlayer.NickName);

            progressLabel.SetActive(true);
            controlPanel.SetActive(false);

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRoom(roomName.ToUpper());
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
}
