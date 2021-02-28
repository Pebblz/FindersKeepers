using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace com.pebblz.finderskeepers
{
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
}
