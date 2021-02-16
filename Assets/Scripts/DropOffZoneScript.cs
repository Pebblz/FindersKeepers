using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;


public class DropOffZoneScript : MonoBehaviourPunCallbacks
{
    GameObject pickup;


    private void OnTriggerEnter(Collider c)
    {
        //if a Player enters the room
        if (c.tag == "Player")
        {
            pickup = c.GetComponent<PlayerPickUp>().PickUp;
            //then it makes sure to see if the players holding an object
            if (c.GetComponent<PlayerPickUp>().isHoldingOBJ)
            {
                if (c.GetComponent<PlayerPickUp>().PickUp.GetComponent<PickUpAbles>().IsThisOBJForPoints)
                {
                    //then it'll encroment the score by 1 
                    c.GetComponent<Player>().score += 1;
                    if (c.GetComponent<PlayerPickUp>().PickUp != null)
                    {
                
       
                        FindObjectOfType<TodoList>().ObjectFound(c.GetComponent<PlayerPickUp>().PickUp.GetComponent<PickUpAbles>());

                        /*c.*/
                        GetComponent<PhotonView>().RPC("ResetDropOffPos", RpcTarget.All);
                        c.GetComponent<PlayerPickUp>().DropPickUp();

                    }
                }
            }
            //if(c.tag != "Player" && c.GetComponent<PickUpAbles>() != null)
            //{
            //    if(c.GetComponent<PickUpAbles>().PlayerThatPickUpOBJ != null && c.GetComponent<PickUpAbles>().throwIntoZoneTimer > 0)
            //    {
            //        c.GetComponent<PickUpAbles>().PlayerThatPickUpOBJ.GetComponent<Player>().score += 1;

            //        FindObjectOfType<TodoList>().ObjectFound(c.GetComponent<PickUpAbles>());

            //        GetComponent<PhotonView>().RPC("ResetDropOffPos", RpcTarget.All);
            //    }
            //}
        }
    }
    [PunRPC]
    public void ResetDropOffPos()
    {
        if (pickup != null)
        {
            pickup.GetComponent<Rigidbody>().velocity = Vector3.zero;
            pickup.GetComponent<Rigidbody>().rotation = pickup.GetComponent<PickUpAbles>().startingRot;
            pickup.transform.position = pickup.GetComponent<PickUpAbles>().OriginalPos;
            if (pickup.GetComponent<PickUpAbles>().hasGravity == false)
            {
                pickup.GetComponent<Rigidbody>().useGravity = pickup.GetComponent<PickUpAbles>().hasGravity;
                pickup.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
                pickup.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            }
        }
    }

    [PunRPC]
    public void DestroyDropOff()
    {
        Destroy(pickup);
    }
}
