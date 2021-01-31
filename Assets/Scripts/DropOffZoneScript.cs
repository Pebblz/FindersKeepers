using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;


public class DropOffZoneScript : MonoBehaviourPunCallbacks
{
    //so guess what this little thing does 
    //i'll give you a sec to figure it out 
    //so what it does is if you enter the trigger it does the stuff 

    PhotonView pv;
    private void Start()
    {
        pv = PhotonView.Get(this);   
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
                        pv.RPC("incrementScore", RpcTarget.All, c.gameObject.name);
                        if(c.GetComponent<PlayerPickUp>().PickUp.GetComponent<SoundtrackManager>() != null)
                        {
                            c.GetComponent<PlayerPickUp>()
                                .PickUp.
                                    GetComponent<SoundtrackManager>().resumeOriginalTrack();
                        }

                        //destroy the pickuped obj
                        deleteObjectInDropoffEvent(c.GetComponent<PhotonView>().ViewID);
                        FindObjectOfType<TodoList>().ObjectFound(c.GetComponent<PlayerPickUp>().PickUp.GetComponent<PickUpAbles>());
                    }
                }
            }
        }
    }

/// <summary>
/// Raises an event to delete gameobject for a given photonViewId
/// </summary>
/// <param name="photonViewId">Id of object to be deleted</param>
    private void deleteObjectInDropoffEvent(int photonViewId)
    {
        object[] content = new object[] { photonViewId };
        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(NetworkCodes.DeleteObjectInDropoffCode, content, options, SendOptions.SendReliable);
    }

}
