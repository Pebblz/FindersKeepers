using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayButton : MonoBehaviourPunCallbacks
{
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            if (GetComponent<PhotonView>().IsMine)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
