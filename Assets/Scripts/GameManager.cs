using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;

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
        Finding_Players = 1, //set to 1 for timer functionality
        The_Run,
        The_Game
    }

    static GameState gameState;

    [SerializeField] Transform[] RespawnPoints;
    [SerializeField] TodoList list;

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
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity);
        }
        else if (Player.localInstance == null)
        {
                Debug.Log("Adding player #" + PhotonNetwork.CurrentRoom.PlayerCount);
                
                
                int thatFuckingIdx = (PhotonNetwork.CurrentRoom.PlayerCount > 1)? FindFirstNotUsedSkin() : 0;
                Debug.Log("Loading player skin: " + this.playerPrefabs[thatFuckingIdx].name);

                PhotonNetwork.Instantiate("Players/" + this.playerPrefabs[thatFuckingIdx].name, new Vector3(0f, 5f, 0f), Quaternion.identity);
                var props = new ExitGames.Client.Photon.Hashtable();
                props.Add("skin", this.playerPrefabs[thatFuckingIdx].name);
                PhotonNetwork.PlayerList[PhotonNetwork.CurrentRoom.PlayerCount -1].SetCustomProperties(props);
        
        }
        else
        {
                Debug.Log("Ignoring PLayer load");
        }

        startButton = GameObject.FindGameObjectWithTag("StartButton");
        if(PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
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

        //TODO: Change to game scene
        PhotonNetwork.LoadLevel("Lobby_" + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public void LoadGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Main Game");
        }
    }


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    #region In game Scene Management
    /// <summary>
    /// Continues the game without having to swap between scenes due to how the online portion works
    /// </summary>
    public void Continue()
    {
        gameState++; //increment gamestate

        if (gameState == GameState.The_Run)
        {
            //rerandomize the room that way the people that join earlier do not get an advantage
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); //just to rerandomize the room, cant just call the roomrandomization function otherwise it will just rerandomize the exact same way as before.
        }
        else if ((int)gameState > 3)
        { //if gameover
            gameState = GameState.Finding_Players; //static so this variable must be manually reset
            //SceneManager.LoadScene("EndScreen"); //end of game load endscreen   scene doesnt exist yet
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
