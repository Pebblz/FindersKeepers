using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class DisableButtonForNonMaster : MonoBehaviourPunCallbacks
{

    void Update()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            gameObject.SetActive(false);
        } else
        {
            gameObject.SetActive(true);
        }
    }
}
