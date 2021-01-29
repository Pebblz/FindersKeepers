using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class PickUpAbles : MonoBehaviourPunCallbacks, IPunObservable
{
    //an [] of Player objs
    GameObject[] Player = new GameObject[4];
    Vector3 latestPos;
    Quaternion latestRot;
    bool valuesReceived = false;
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
            if (Player[i].GetComponent<PlayerPickUp>().PickUp == this.gameObject.transform)
            {
                transform.position = Player[i].transform.position + new Vector3(0, 1, 0);
            }
            if (!photonView.IsMine && valuesReceived)
            {
                transform.position = Vector3.Lerp(transform.position, latestPos, Time.deltaTime * 5);
                transform.rotation = Quaternion.Lerp(transform.rotation, latestRot, Time.deltaTime * 5);
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
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            //data recieved from other players
            //IsPickedUped = (bool)stream.ReceiveNext();
            //PlayerThatPickUpOBJ.transform.position = (Vector3)stream.ReceiveNext();
            latestPos = (Vector3)stream.ReceiveNext();
            latestRot = (Quaternion)stream.ReceiveNext();
            valuesReceived = true;
        }

    }
}
