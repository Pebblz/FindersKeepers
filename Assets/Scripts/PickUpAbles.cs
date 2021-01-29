using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class PickUpAbles : MonoBehaviourPunCallbacks, IPunObservable
{
    //an [] of Player objs
    GameObject[] Player = new GameObject[4];

    bool IsPickedUped;
    GameObject PlayerThatPickUpOBJ;
    //this is an awake because it'll do this whenever this object gets spawned
    void Awake()
    {
        //finds the players
        Player = GameObject.FindGameObjectsWithTag("Player");
    }

    void Update()
    {

        for (int i = 0; i < Player.Length; i++)
        {
            //checks how close the players are to the obj 
            if (Vector3.Distance(this.gameObject.transform.position, Player[i].transform.position) < 5)
            {
                //if he's getting ready to pick up the obj
                if (Player[i].GetComponent<PlayerPickUp>().isHoldingOBJ == false &&
                    Player[i].GetComponent<PlayerPickUp>().isPickingUpOBJ == true && IsPickedUped == false)
                {
                    //does this stuff

                    Player[i].GetComponent<PlayerPickUp>().SetPickUpOBJ(this.gameObject);
                    Player[i].GetComponent<PlayerPickUp>().isHoldingOBJ = true;
                    PlayerThatPickUpOBJ = Player[i];
                    //Object.FindObjectOfType<TodoList>().PickUpObject(this); //tells the list it was picked up
                    IsPickedUped = true;
                }
            }


        }
        if (photonView.IsMine)
        {

        }
        else
        {
            photonView.RPC("MovingOBJ", RpcTarget.All);
        }
    }
    [PunRPC]
    public void MovingOBJ()
    {
        if (PlayerThatPickUpOBJ != null)
        {
            if (PlayerThatPickUpOBJ.GetComponent<PlayerPickUp>().PickUp == this.gameObject)
            {
                transform.position = PlayerThatPickUpOBJ.transform.position + new Vector3(0, 1, 0);
            }
        }
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

        if (stream.IsWriting)
        {
            //data that gets sent to other players
            //stream.SendNext(IsPickedUped);
            //stream.SendNext(PlayerThatPickUpOBJ);
        }
        else
        {
            //data recieved from other players
            //IsPickedUped = (bool)stream.ReceiveNext();

        }

    }
}
