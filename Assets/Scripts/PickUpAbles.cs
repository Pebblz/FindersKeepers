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
    GameObject PlayerThatPickUpOBJ;
    public PhotonView pv;

    //this is an awake because it'll do this whenever this object gets spawned
    void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        

        if (photonView.IsMine)
        {

            
        }
        if (Vector3.Distance(this.gameObject.transform.position, player.transform.position) < 5)
        {

            if (player.GetComponent<PlayerPickUp>().isHoldingOBJ == false &&
                            player.GetComponent<PlayerPickUp>().isPickingUpOBJ == true && IsPickedUped == false)
            {
                player.GetComponent<PlayerPickUp>().SetPickUpOBJ(this.gameObject);
                player.GetComponent<PlayerPickUp>().isHoldingOBJ = true;
                PlayerThatPickUpOBJ = player;
                pv.TransferOwnership(PhotonNetwork.LocalPlayer);
                IsPickedUped = true;
            }


            if (this.gameObject == player.GetComponent<PlayerPickUp>().PickUp)
            {
                //pv.RPC("MovePickUp", RpcTarget.AllViaServer);
                gameObject.transform.position = player.transform.position + new Vector3(0, 1, 0);
            }
        }

    }

    [PunRPC]
    public void MovePickUp()
    {
        gameObject.transform.position = player.transform.position + new Vector3(0, 1, 0);         
    }


    public void DropPickUp()
    {
        transform.position = PlayerThatPickUpOBJ.transform.position + PlayerThatPickUpOBJ.transform.forward * 1.5f;
        transform.rotation = new Quaternion(0, PlayerThatPickUpOBJ.transform.rotation.y, 0, PlayerThatPickUpOBJ.transform.rotation.w);
        IsPickedUped = false;
        PlayerThatPickUpOBJ = null;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        //if (stream.IsWriting)
        //{
        //    //data that gets sent to other players
        //    stream.SendNext(this.gameObject.transform.position);
        //    //stream.SendNext(PlayerThatPickUpOBJ);
        //}
        //else
        //{
        //    //data recieved from other players
        //    this.gameObject.transform.position = (Vector3)stream.ReceiveNext();
        //}

    }
}
