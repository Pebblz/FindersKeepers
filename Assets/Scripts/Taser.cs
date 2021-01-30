using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Taser : MonoBehaviourPunCallbacks
{
    //this is so the player doesn't shoot himself 
    public GameObject PlayerWhoShotThis;
    PhotonView pv;
    float DestroyTimer = .7f;
    void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    void Update()
    {
        DestroyTimer -= Time.deltaTime;
        if (DestroyTimer <= 0)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
    private void OnTriggerEnter(Collider col)
    {

        if (col.tag == "Player" && col.gameObject != PlayerWhoShotThis)
        {
            col.GetComponent<Player>().StunPlayer();
            col.GetComponent<PlayerPickUp>().DropOBJ();
            if (!PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("DestroyOBJ", RpcTarget.All);
            } else
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }

    }

    public void DestroyOBJ()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }
}
