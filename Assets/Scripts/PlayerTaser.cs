﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class PlayerTaser : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject taserOBJ;

    int TasersLeft = 2;

    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.E) && TasersLeft > 0)
            {
                GetComponent<Player>().isFiring = true;
                shootTaser();
            }
        }
    }
    void shootTaser()
    {

        //this spawns the bullet 
        GameObject temp = PhotonNetwork.Instantiate("Test_Taser", this.gameObject.transform.position + new Vector3(0, 1.2f,0), Quaternion.identity);
        //this makes sure player doesn't shoot himself 
        temp.GetComponent<Taser>().PlayerWhoShotThis = gameObject;
        //bullet go forward
        temp.GetComponent<Rigidbody>().velocity = transform.forward * 10f;
        //you lose a taser if you shoot a taser
        TasersLeft -= 1;
        GetComponent<Player>().isFiring = false;
    }
}
