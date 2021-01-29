using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class PickUpAbles : MonoBehaviourPunCallbacks, IPunObservable
{
    //an [] of Player objs
    // GameObject[] Player = new GameObject[4];
    public GameObject player;
    bool IsPickedUped;
    GameObject PlayerThatPickUpOBJ;
    //Vector3 TextPos;
    public PhotonView pv;

    //this is an awake because it'll do this whenever this object gets spawned
    void Awake()
    {
        //finds the players
        // Player = GameObject.FindGameObjectsWithTag("Player");
        player = GameObject.FindGameObjectWithTag("Player");
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
            if (Vector3.Distance(this.gameObject.transform.position, player.transform.position) < 5)
            {
                if (player.GetComponent<PlayerPickUp>().isHoldingOBJ == false &&
                                player.GetComponent<PlayerPickUp>().isPickingUpOBJ == true && IsPickedUped == false)
                {
                    player.GetComponent<PlayerPickUp>().SetPickUpOBJ(this.gameObject);
                    player.GetComponent<PlayerPickUp>().isHoldingOBJ = true;
                    PlayerThatPickUpOBJ = player;
                    IsPickedUped = true;
                }

                if (this.gameObject == player.GetComponent<PlayerPickUp>().PickUp)
                {
                    pv.RPC("MovePickUp", RpcTarget.AllViaServer);
                }
                //for (int i = 0; i < Player.Length; i++)
                //{
                //    //checks how close the players are to the obj 
                //    if (Vector3.Distance(this.gameObject.transform.position, Player[i].transform.position) < 5)
                //    {
                //        //if he's getting ready to pick up the obj
                //        if (Player[i].GetComponent<PlayerPickUp>().isHoldingOBJ == false &&
                //            Player[i].GetComponent<PlayerPickUp>().isPickingUpOBJ == true && IsPickedUped == false)
                //        {

                //            Player[i].GetComponent<PlayerPickUp>().SetPickUpOBJ(this.gameObject);
                //            Player[i].GetComponent<PlayerPickUp>().isHoldingOBJ = true;
                //            PlayerThatPickUpOBJ = Player[i];
                //            //Object.FindObjectOfType<TodoList>().PickUpObject(this); //tells the list it was picked up
                //            IsPickedUped = true;
                //        }
                //    }


                //}
            }
        
        
    }

    [PunRPC]
    public void MovePickUp()
    {
        gameObject.transform.position = PlayerThatPickUpOBJ.transform.position + new Vector3(0, 1, 0);
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
