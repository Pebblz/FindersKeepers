using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace com.pebblz.finderskeepers
{
    public class Taser : MonoBehaviourPunCallbacks
    {
        //this is so the player doesn't shoot himself 
        public GameObject PlayerWhoShotThis;
        float DestroyTimer = .7f;
        void Update()
        {
            //a timer for destroying the taser
            DestroyTimer -= Time.deltaTime;

            if (DestroyTimer <= 0)
            {
                Destroy(this.gameObject);
            }
        }
        private void OnTriggerEnter(Collider col)
        {
            //the taser spawns inside the hitbox of the player shooting it
            // so i have it set the player who shot it to the col gameobject 
            //seeing the playerwhoshotthis never gets set for anyone but the host
            if (PlayerWhoShotThis == null)
            {
                PlayerWhoShotThis = col.gameObject;
            }
            //checks to see if you hit a player and that player isn't yourself 
            if (col.tag == "Player" && col.gameObject != PlayerWhoShotThis)
            {
                //stuns the player
                col.GetComponent<Player>().StunPlayer();
                //if the players holding a obj he drops it 
                col.GetComponent<PlayerPickUp>().DropOBJ();
                //changes ownership if you're not the owner
                if (GetComponent<PhotonView>().Owner != PhotonNetwork.LocalPlayer)
                {
                    ChangeOwnerShip();
                }
                GetComponent<PhotonView>().RPC("DestroyTaser", RpcTarget.All);

            }
            if (col.GetComponent<Player>() == null)
            {
                //changes ownership if you're not the owner
                if (GetComponent<PhotonView>().Owner != PhotonNetwork.LocalPlayer)
                {
                    ChangeOwnerShip();
                }
                GetComponent<PhotonView>().RPC("DestroyTaser", RpcTarget.All);
            }


        }
        public void ChangeOwnerShip()
        {
            //changes ownership of the taser to the local player
            GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
        }
        [PunRPC]
        void DestroyTaser()
        {
            //this Destroys the taser
            PhotonView.Destroy(PlayerWhoShotThis.GetComponent<PlayerTaser>().taserInstance);
        }
    }
}
