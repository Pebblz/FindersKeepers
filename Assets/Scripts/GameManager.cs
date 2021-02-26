using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class GameManager : MonoBehaviourPunCallbacks
{
    /*Flower box
     * 
     * Edited by: Pat Naatz
     * Added:
     *  Continue function
     *  Respawn function
     *  SetTime function
     *  ListActivated function
     *  GameState enum
     *  InSeconds Struct for play time
     */

    static public GameManager Instance;
    public bool OnlyOnePlayer = false;
    private GameObject instance;
    [Tooltip("The prefab used to load multiple players")]
    public GameObject[] playerPrefabs;
    [SerializeField]
    private GameObject playerPrefab;
    public GameObject startButton;
    public bool isGameScene;
    bool PressPlayButtonOnce;


    enum GameState
    {//Enum Gamestate instead of Scene management because we dont want to swap scenes to avoid online issues
        The_Run = 2,
        The_Game
    }
    static GameState gameState = GameState.The_Run; //static to hold between scenes, the first time this variable is used Thr Run will be relevant
    
    //Game time class
    [System.Serializable] struct InSeconds
    {
        public int RunTime, PlayTime;
    }
    [SerializeField] InSeconds seconds;

    [SerializeField] Transform[] RespawnPoints;
    [SerializeField] TodoList list;


    public static GameObject[] Randomize(IEnumerable<GameObject> source)
    {
        System.Random rnd = new System.Random();
        return source.OrderBy<GameObject, int>((item) => rnd.Next()).ToArray();
    }


    void Start()
    {
        if (isGameScene)
        {
            PhotonView p = PhotonView.Get(this);
            p.RPC("PlayGameTheme", RpcTarget.All);
        }

        GameObject[] playerfabs = Resources.LoadAll<GameObject>("Players").ToArray();
        playerPrefabs = GameManager.Randomize(playerfabs);

        Instance = this;
        if (OnlyOnePlayer)
        {
            PhotonNetwork.Instantiate("Players/" + playerPrefab.name, new Vector3(0f, 1f, 0f), Quaternion.identity);
       
        }
        else if (Player.localInstance == null)
        {
            Debug.Log("Adding player #" + PhotonNetwork.CurrentRoom.PlayerCount);


            int idx = (PhotonNetwork.CurrentRoom.PlayerCount > 1) ? FindFirstNotUsedSkin() : 0;
            Debug.Log("Loading player skin: " + this.playerPrefabs[idx].name);


            PhotonNetwork.Instantiate("Players/" + this.playerPrefabs[idx].name, new Vector3(0f, 1f, 0f), Quaternion.identity);

            var props = new ExitGames.Client.Photon.Hashtable();
            props.Add("skin", this.playerPrefabs[idx].name);
            PhotonNetwork.PlayerList[PhotonNetwork.CurrentRoom.PlayerCount - 1].SetCustomProperties(props);

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
        foreach (var p in this.playerPrefabs)
        {
            allPlayerTypes.Add(p.name);
        }

        List<string> activePlayerTypes = getActivePlayerTypes();
        string str = "";
        foreach (string s in activePlayerTypes)
        {
            str += s + ", ";
        }
        Debug.Log("Active Player Types: " + str);
        if (activePlayerTypes.Count == 1)
        {
            return 0;
        }
        var firstItem = allPlayerTypes.Except(activePlayerTypes).ToArray()[0];
        for (int i = 0; i < allPlayerTypes.Count; i++)
        {
            if (allPlayerTypes[i] == firstItem)
            {
                return i;
            }
        }
        return 0;
    }

    public List<string> getActivePlayerTypes()
    {
        List<string> typesOfColors = new List<string>();
        foreach (var player in PhotonNetwork.PlayerList)
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

        if (isGameScene)
        {
            return;
        }
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("A non master client attempted to load level");
        }
        NetworkSceneChangedRaiseEvent();
        PhotonNetwork.LoadLevel("Lobby_" + PhotonNetwork.CurrentRoom.PlayerCount);

    }

    public void LoadGame()
    {

        if (PhotonNetwork.IsMasterClient && PressPlayButtonOnce == false)
        {
            //MusicChangeRaiseEvent();
            NetworkSceneChangedRaiseEvent();
            PhotonNetwork.LoadLevel("Main Game");
            PressPlayButtonOnce = true;


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
        bool worked = PhotonNetwork.RaiseEvent(NetworkCodes.ChangeToGameMusicEvent, content, options, SendOptions.SendReliable);
        Debug.Log("Music Event: " + worked);
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
            foreach (Player player in FindObjectsOfType<Player>())
            {
                player.GetComponent<Player_Movement>().enabled = false;
                player.freeLookCam.SetActive(false);
            }
            FindObjectOfType<Camera>().gameObject.SetActive(false);
            PhotonNetwork.LoadLevel("WinOrLose"); //end of game load endscreen   scene doesnt exist yet
        }
        else
        {
            Respawn();
            //the timer is handled internally
        }
    }

    void Respawn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //assign neccesary variables
            Player[] players = FindObjectsOfType<Player>();
            int assignedRespawn = 0;

            foreach (Player player in players)
            {
                //move player to assigned position
                player.photonView.RPC("MoveToHere", RpcTarget.All, RespawnPoints[assignedRespawn].position);

                //setup for next rotation
                assignedRespawn++;
            }
        }

        //this funciton is only called once during the transfer between The_Run and The_Game
        //Start the todo list
        Object.FindObjectOfType<TodoList>().Active();
    }
    #endregion

    #region in game data checks
    /// <summary>
    /// returns the time this GameState should have ot be played out
    /// </summary>
    /// <returns></returns>
    public int setTime()
    {//for timer reseting
        switch (gameState)
        {
            case GameState.The_Run:
                return seconds.RunTime;
            case GameState.The_Game:
                return seconds.PlayTime;
        }
        return 1;
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
