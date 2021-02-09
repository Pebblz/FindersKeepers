using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayButton : MonoBehaviourPunCallbacks
{
    void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {

            this.gameObject.SetActive(false);

        }
    }
}
