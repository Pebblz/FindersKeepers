using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class PickUpAbles : MonoBehaviourPunCallbacks, IPunObservable
{
    public GameObject player;
    GameObject[] gs = new GameObject[4];
    public bool IsPickedUped;
    public GameObject PlayerThatPickUpOBJ;
    public PhotonView pv;
    Vector3 OriginalPos;
    //this is an awake because it'll do this whenever this object gets spawned
    void Awake()
    {
        OriginalPos = transform.position;
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (Vector3.Distance(this.gameObject.transform.position, player.transform.position) < 5)
        {

            if (player.GetComponent<PlayerPickUp>().isHoldingOBJ == false &&
                player.GetComponent<PlayerPickUp>().isPickingUpOBJ == true && IsPickedUped == false)
            {
                player.GetComponent<PlayerPickUp>().SetPickUpOBJ(this.gameObject);
                player.GetComponent<PlayerPickUp>().isHoldingOBJ = true;
                PlayerThatPickUpOBJ = player;
                ChangeOwnerShip();
                IsPickedUped = true;
            }


            if (this.gameObject == player.GetComponent<PlayerPickUp>().PickUp)
            {
                gameObject.transform.position = player.transform.position + new Vector3(0, 2.5f, 0);
            }
            
        }

        if (this.transform.position.y <= -100)
        {
            ResetPos();
        }
    }
    //changes the ownership of the GameObject
    public void ChangeOwnerShip()
    {
        pv.TransferOwnership(PhotonNetwork.LocalPlayer);
    }
    //Drops the GameObject
    public void DropPickUp()
    {
        transform.position = PlayerThatPickUpOBJ.transform.position + new Vector3(0, .5f, 0) + PlayerThatPickUpOBJ.transform.forward * 1.5f;
        transform.rotation = new Quaternion(0, PlayerThatPickUpOBJ.transform.rotation.y, 0, PlayerThatPickUpOBJ.transform.rotation.w);
        IsPickedUped = false;
        PlayerThatPickUpOBJ = null;
    }
    //Resets the Position of the GameObject
    //to the position that it started at 
    void ResetPos()
    {       
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = OriginalPos;
        ChangeOwnerShip();
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            stream.SendNext(IsPickedUped);
            stream.SendNext(OriginalPos);
        }
        else
        {
            IsPickedUped = (bool)stream.ReceiveNext();
            OriginalPos = (Vector3)stream.ReceiveNext();
        }

    }
}
