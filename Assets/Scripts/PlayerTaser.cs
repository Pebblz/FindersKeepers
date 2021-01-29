using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class PlayerTaser : MonoBehaviourPunCallbacks, IPunObservable
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
        GameObject temp = Instantiate(taserOBJ, this.gameObject.transform.position, Quaternion.identity);
        //this makes sure player doesn't shoot himself 
        temp.GetComponent<Taser>().PlayerWhoShotThis = gameObject;
        //bullet go forward
        temp.GetComponent<Rigidbody>().velocity = transform.forward * 10f;
        //you lose a taser if you shoot a taser
        TasersLeft -= 1;
        GetComponent<Player>().isFiring = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
