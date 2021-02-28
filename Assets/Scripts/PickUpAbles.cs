using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace com.pebblz.finderskeepers
{
    public class PickUpAbles : MonoBehaviourPunCallbacks, IPunObservable
    {
        public GameObject player;
        public bool IsPickedUped;
        public GameObject PlayerThatPickUpOBJ;
        public PhotonView pv;
        public bool useGravity;
        public Vector3 OriginalPos;
        public bool IsThisOBJForPoints;
        public Quaternion startingRot;
        public bool hasGravity = true;
        public Sprite image;
        public float throwIntoZoneTimer;
        //this is an awake because it'll do this whenever this object gets spawned
        void Awake()
        {
            startingRot = transform.rotation;
            GetComponent<Rigidbody>().useGravity = hasGravity;
            OriginalPos = transform.position;
            pv = GetComponent<PhotonView>();
        }

        void Update()
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");

                //Don't do anything if a player is not found
                if (player == null)
                {
                    return;
                }
            }
            if (transform.parent != null)
            {
                photonView.RPC("unparent", RpcTarget.All);
            }

            if (this.gameObject == player.GetComponent<PlayerPickUp>().PickUp && IsPickedUped)
            {
                gameObject.transform.position = player.transform.position + new Vector3(0, 2.5f, 0);
            }
            if (this.transform.position.y <= -100)
            {
                pv.RPC("ResetPos", RpcTarget.All);
            }
        }
        //changes the ownership of the GameObject
        public void ChangeOwnerShip()
        {
            pv.TransferOwnership(PhotonNetwork.LocalPlayer);
        }

        [PunRPC]
        public void unparent()
        {
            this.gameObject.transform.parent = null;
        }

        [PunRPC]
        public void DestroyThis()
        {
            Destroy(this.gameObject);
        }
        //Drops the GameObject
        public void DropPickUp()
        {
            transform.position = PlayerThatPickUpOBJ.transform.position + new Vector3(0, .5f, 0) + PlayerThatPickUpOBJ.transform.forward * 1.5f;
            transform.rotation = new Quaternion(0, PlayerThatPickUpOBJ.transform.rotation.y, 0, PlayerThatPickUpOBJ.transform.rotation.w);
            IsPickedUped = false;
            PlayerThatPickUpOBJ = null;
        }
        //Resets the Position of the GameObject
        //to the position that it started at 
        [PunRPC]
        public void ResetPos()
        {

            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.GetComponent<Rigidbody>().rotation = startingRot;
            this.transform.position = OriginalPos;
            if (hasGravity == false)
            {
                GetComponent<Rigidbody>().useGravity = hasGravity;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            }
        }
        //this is for pictures, or anything that we would want to float
        //but still be pickupable
        public void UseGravity(bool does_it)
        {
            if (GetComponent<Rigidbody>().useGravity == false)
            {
                GetComponent<Rigidbody>().useGravity = does_it;
            }
        }
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {

            if (stream.IsWriting)
            {
                stream.SendNext(IsPickedUped);
                stream.SendNext(useGravity);
            }
            else
            {
                IsPickedUped = (bool)stream.ReceiveNext();
                this.GetComponent<Rigidbody>().useGravity = (bool)stream.ReceiveNext();
            }

        }
    }
}