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
    void Start()
    {
        // do some linq stuff to order the players
        var playerfabs  = Resources.LoadAll<GameObject>("Players");
        var temp = from s in playerfabs
                        orderby s.name descending
                        select s;
        playerPrefabs = temp.ToArray();


        Instance = this;
        if (OnlyOnePlayer)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity);
        }
        else if (Player.localInstance == null)
        {
                Debug.Log("Adding player #" + PhotonNetwork.CurrentRoom.PlayerCount);
                int thatFuckingIdx = PhotonNetwork.CurrentRoom.PlayerCount;
                PhotonNetwork.Instantiate("Players/" + this.playerPrefabs[thatFuckingIdx].name, new Vector3(0f, 5f, 0f), Quaternion.identity);
        }
        else
        {
                Debug.Log("Ignoring PLayer load");
        }
        
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


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
