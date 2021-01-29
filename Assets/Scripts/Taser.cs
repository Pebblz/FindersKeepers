using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Taser : MonoBehaviourPunCallbacks
{
    //this is so the player doesn't shoot himself 
    public GameObject PlayerWhoShotThis;

    float DestroyTimer = .7f;
    void Update()
    {
        DestroyTimer -= Time.deltaTime;
        if (DestroyTimer <= 0)
            PhotonNetwork.Destroy(this.gameObject);
    }
    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player" && col.gameObject != PlayerWhoShotThis)
        {
            col.GetComponent<Player>().StunPlayer();
            col.GetComponent<PlayerPickUp>().DropOBJ();
            PhotonNetwork.Destroy(this.gameObject);
        }
        
    }
}
