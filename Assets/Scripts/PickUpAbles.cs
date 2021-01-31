using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using Photon.Realtime;
using ExitGames.Client.Photon;


public class PickUpAbles : MonoBehaviourPunCallbacks, IPunObservable
{
    public GameObject player;
    GameObject[] gs = new GameObject[4];
    public bool IsPickedUped;
    public GameObject PlayerThatPickUpOBJ;
    public PhotonView pv;
    public bool useGravity;
    Vector3 OriginalPos;
    public bool IsThisOBJForPoints;
    Quaternion startingRot;
    [SerializeField]
    bool hasGravity = true;
    //this is an awake because it'll do this whenever this object gets spawned
    void Awake()
    {
        startingRot = transform.rotation;
        GetComponent<Rigidbody>().useGravity = hasGravity;
        OriginalPos = transform.position;
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        //added this because of a error when a player leaves and rejoins
        if (player != null)
        {
            if (Vector3.Distance(this.gameObject.transform.position, player.transform.position) < 3.5f)
            {

                if (player.GetComponent<PlayerPickUp>().isHoldingOBJ == false &&
                    player.GetComponent<PlayerPickUp>().isPickingUpOBJ == true && IsPickedUped == false)
                {
                    GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    UseGravity(true);
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
        }

        if (this.transform.position.y <= -100)
        {
            pv.RPC("ResetPos", RpcTarget.All);
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
    [PunRPC]
    void ResetPos()
    {

        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().rotation = startingRot;
        transform.position = OriginalPos;
        if (hasGravity == false)
        {
            GetComponent<Rigidbody>().useGravity = hasGravity;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        }
   
        //ChangeOwnerShip();
    }
    //this is for pictures, or anything that we would want to float
    //but still be pickupable
    void UseGravity(bool does_it)
    {
        if (GetComponent<Rigidbody>().useGravity == false)
        {
            GetComponent<Rigidbody>().useGravity = does_it;
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            stream.SendNext(IsPickedUped);
            stream.SendNext(useGravity);
            //stream.SendNext(OriginalPos);
        }
        else
        {
            IsPickedUped = (bool)stream.ReceiveNext();
            this.GetComponent<Rigidbody>().useGravity = (bool)stream.ReceiveNext();
            //OriginalPos = (Vector3)stream.ReceiveNext();
        }

    }
}
