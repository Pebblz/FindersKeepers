using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    static public GameManager Instance;

    private GameObject instance;
    [Tooltip("The prefab used to load multiple players")]

    [SerializeField]
    private GameObject playerPrefab;
    void Start()
    {
        Instance = this;
        if (playerPrefab == null)
        {
            Debug.LogError("No Player prefab provided");
        }
        else
        {

            if (Player.localInstance == null)
            {
                Debug.Log("Adding player #" + PhotonNetwork.CurrentRoom.PlayerCount);
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity);
            }
            else
            {
                Debug.Log("Ignoring PLayer load");
            }
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
