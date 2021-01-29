using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class PickUpAbles : MonoBehaviourPunCallbacks, IPunObservable
{
    //an [] of Player objs
    GameObject Player;

    bool IsPickedUped;
    GameObject PlayerThatPickUpOBJ;
    //this is an awake because it'll do this whenever this object gets spawned
    void Awake()
    {
        //finds the players
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {


        //checks how close the players are to the obj 
        if (Vector3.Distance(this.gameObject.transform.position, Player.transform.position) < 5)
        {
            //if he's getting ready to pick up the obj
            if (Player.GetComponent<PlayerPickUp>().isHoldingOBJ == false &&
                Player.GetComponent<PlayerPickUp>().isPickingUpOBJ == true && IsPickedUped == false)
            {
                //does this stuff
                transform.parent = Player.transform;
                transform.position = Player.transform.position + new Vector3(0, 1, 0);
                Player.GetComponent<PlayerPickUp>().SetPickUpOBJ(this.gameObject);
                Player.GetComponent<PlayerPickUp>().isHoldingOBJ = true;
                PlayerThatPickUpOBJ = Player;
                //Object.FindObjectOfType<TodoList>().PickUpObject(this); //tells the list it was picked up
                IsPickedUped = true;
            }
        }
    }
    public void DropPickUp()
    {
        transform.position = PlayerThatPickUpOBJ.transform.position + PlayerThatPickUpOBJ.transform.forward * 1.5f;
        transform.rotation = new Quaternion(0, PlayerThatPickUpOBJ.transform.rotation.y, 0, PlayerThatPickUpOBJ.transform.rotation.w);
        transform.parent = null;
        IsPickedUped = false;
        PlayerThatPickUpOBJ = null;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            //data that gets sent to other players
            stream.SendNext(IsPickedUped);
            stream.SendNext(this.transform.position);

        }
        else
        {
            //data recieved from other players
            IsPickedUped = (bool)stream.ReceiveNext();
            transform.position = (Vector3)stream.ReceiveNext();


        }

    }
}
