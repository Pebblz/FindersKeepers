using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DropOffZoneScript : MonoBehaviourPunCallbacks
{
    //so guess what this little thing does 
    //i'll give you a sec to figure it out 
    //so what it does is if you enter the trigger it does the stuff 
    

    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider c)
    {
        //if a Player enters the room
        if(c.tag == "Player")
        {
            if (c.GetComponent<PhotonView>().IsMine)
            {
                //then it makes sure to see if the players holding an object
                if (c.GetComponent<PlayerPickUp>().isHoldingOBJ == true)
                {
                    if (c.GetComponent<PlayerPickUp>().PickUp.GetComponent<PickUpAbles>().IsThisOBJForPoints == true)
                    {
                        //then it'll encroment the score by 1 
                        c.GetComponent<Player>().score += 1;
                        //destroy the pickuped obj
                        this.photonView.RPC("DestroyPickUp", RpcTarget.All);
                        //and set his holding obj to false
                        c.GetComponent<PlayerPickUp>().isHoldingOBJ = false;
                    }
                }
            }
        }
    }

}
