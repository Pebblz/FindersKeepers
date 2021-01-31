﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class GameManager : MonoBehaviourPunCallbacks
{
    static public GameManager Instance;
    public bool OnlyOnePlayer = false;
    private GameObject instance;
    [Tooltip("The prefab used to load multiple players")]
    public GameObject[] playerPrefabs;
    [SerializeField]
    private GameObject playerPrefab;
    public GameObject startButton;



    enum GameState
    {//Enum Gamestate instead of Scene management because we dont want to swap scenes to avoid online issues
        //Finding_Players = 1, //set to 1 for timer functionality
        The_Run = 2,
        The_Game
    }

    static GameState gameState;

    [SerializeField] Transform[] RespawnPoints;
    [SerializeField] TodoList list;
   // [SerializeField]
    //private GameObject speedPowerPrefab;

    public static GameObject[] Randomize(IEnumerable<GameObject> source)
    {
        System.Random rnd = new System.Random();
        return source.OrderBy<GameObject, int>((item) => rnd.Next()).ToArray();
    }


    void Start()
    {
        // do some linq stuff to order the players
        var playerfabs  = Resources.LoadAll<GameObject>("Players");
        var temp = from s in playerfabs
                        orderby s.name descending
                        select s;
        playerPrefabs = temp.ToArray();
        playerPrefabs = GameManager.Randomize(playerPrefabs);


        Instance = this;
        if (OnlyOnePlayer)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0f, 1f, 0f), Quaternion.identity);
        }
        else if (Player.localInstance == null)
        {
                Debug.Log("Adding player #" + PhotonNetwork.CurrentRoom.PlayerCount);
                
                
                int thatFuckingIdx = (PhotonNetwork.CurrentRoom.PlayerCount > 1)? FindFirstNotUsedSkin() : 0;
                Debug.Log("Loading player skin: " + this.playerPrefabs[thatFuckingIdx].name);


            PhotonNetwork.Instantiate("Players/" + this.playerPrefabs[thatFuckingIdx].name, new Vector3(0f, 1f, 0f), Quaternion.identity);

            var props = new ExitGames.Client.Photon.Hashtable();
                props.Add("skin", this.playerPrefabs[thatFuckingIdx].name);
                PhotonNetwork.PlayerList[PhotonNetwork.CurrentRoom.PlayerCount -1].SetCustomProperties(props);
        
        }
        else
        {

            Debug.Log("Ignoring PLayer load");
        }

        if (startButton != null)
        {
            startButton = GameObject.FindGameObjectWithTag("StartButton");
            if (PhotonNetwork.IsMasterClient)
            {
                startButton.SetActive(true);
            }
            else
            {
                startButton.SetActive(false);
            }
        }
    }

    public int FindFirstNotUsedSkin()
    {
        
        List<string> allPlayerTypes = new List<string>();
        foreach(var p in this.playerPrefabs)
        {
            allPlayerTypes.Add(p.name);
        }

        List<string> activePlayerTypes = getActivePlayerTypes();
        string str = "";
        foreach(string s in activePlayerTypes)
        {
            str += s + ", ";
        }
        Debug.Log("Active Player Types: " + str);
        if(activePlayerTypes.Count == 1)
        {
            return 0;
        }
        var firstItem = allPlayerTypes.Except(activePlayerTypes).ToArray()[0];
        for(int i = 0; i < allPlayerTypes.Count; i++)
        {
            if(allPlayerTypes[i] == firstItem)
            {
                return i;
            }
        }
        return 0;
    }

    public List<string> getActivePlayerTypes()
    {
        List<string> typesOfColors = new List<string>();
        foreach(var player in PhotonNetwork.PlayerList)
        {
            typesOfColors.Add((string)player.CustomProperties["skin"]);
        }
        return typesOfColors;
    }

    #region photon callbacks
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            LoadArena();
        }
    }


    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            LoadArena();
        }
    }

    #endregion

    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("A non master client attempted to load level");
        }
        NetworkSceneChangedRaiseEvent();
        PhotonNetwork.LoadLevel("Lobby_" + PhotonNetwork.CurrentRoom.PlayerCount);
       
    }

    public void LoadGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            NetworkSceneChangedRaiseEvent();
            MusicChangeRaiseEvent();
            PhotonNetwork.LoadLevel("Main Game");
            
            
        }
    }


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    #region Network Events
    // the flag for raising a Network Event
    
    private void NetworkSceneChangedRaiseEvent()
    {
        object[] content = new object[] { };
        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(NetworkCodes.NetworkSceneChangedEventCode, content, options, SendOptions.SendReliable);
    }

    private void MusicChangeRaiseEvent()
    {
        object[] content = new object[] { };
        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(NetworkCodes.ChangeToGameMusicEvent, content, options, SendOptions.SendReliable);
    }

    private void RandomRoomEvent()
    {
        object[] content = new object[] { };
        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(NetworkCodes.RandomRoomEventCode, content, options, SendOptions.SendReliable);
    }
    #endregion

    #region In game Scene Management
    /// <summary>
    /// Continues the game without having to swap between scenes due to how the online portion works
    /// </summary>
    public void Continue()
    {
        gameState++; //increment gamestate

        
        if ((int)gameState > 3)
        { //if gameover
            gameState = GameState.The_Run; //static so this variable must be manually reset
            SceneManager.LoadScene("WinOrLose"); //end of game load endscreen   scene doesnt exist yet
        }
        else
        {
            Respawn();
            //the timer is handled internally
        }
    }

    void Respawn()
    {
        //assign neccesary variables
        Player[] players = FindObjectsOfType<Player>();
        int assignedRespawn = 0;

        foreach (Player player in players)
        {
            //move player to assigned position
            player.gameObject.transform.position = RespawnPoints[assignedRespawn].position;
            //player.gameObject.transform.rotation = RespawnPoints[assignedRespawn].rotation;   I dont think we will care about rotation
            assignedRespawn++;
        }

        Debug.Log("hello");
        Object.FindObjectOfType<TodoList>().PrintList();
    }
    #endregion

    #region in game data checks
    /// <summary>
    /// returns the time this GameState should have ot be played out
    /// </summary>
    /// <returns></returns>
    public int setTime()
    {//for timer reseting
        return (int)gameState * 30;
    }

    /// <summary>
    /// Activates or deactivates the list
    /// </summary>
    /// <returns></returns>
    public bool listActive()
    {//for activating the list
        return gameState >= GameState.The_Game;
    }
    #endregion
}
