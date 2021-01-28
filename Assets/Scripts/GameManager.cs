using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    [Tooltip("The prefab used to load multiple players")]
    public GameObject playerPrefab;
    void Start()
    {
    }


    #region photon callbacks
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
    #endregion




    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
