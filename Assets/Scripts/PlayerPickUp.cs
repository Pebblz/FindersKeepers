﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using ExitGames.Client.Photon;

[RequireComponent(typeof(Player))]
public class PlayerPickUp : MonoBehaviourPunCallbacks, IPunObservable, IOnEventCallback
{
    /*Flower Box
     * 
     * Edited By: Pat Naatz
     * 
     * Added Pick Up Sound
     * Added Requirement of player for sound
     */


    public GameObject PickUp;
    public bool isHoldingOBJ = false;
    public bool isPickingUpOBJ = false;
    Rigidbody rb;
    [SerializeField]
    int ThrowForce;
    Animator Anim;
    float pickUpTimer;

    GameObject PickUpSpawner;

    [SerializeField] AudioClip PickUpSound;

    Player player;

    // Start is called before the first frame update
    void Awake()
    {
        Anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        player = GetComponent<Player>();
    }


    // Update is called once per frame
    void Update()
    {

        pickUpTimer -= Time.deltaTime;

        if (photonView.IsMine)
        {

            //if the player holds q
            if (Input.GetButtonDown("Fire1") && pickUpTimer < 0)
            {
                //this checks to see if the players picking up an obj
                if (PickUp == null)
                {
                    isPickingUpOBJ = true;
                    Vector3 start = this.gameObject.transform.position + new Vector3(0, .5f, 0);
                    RaycastHit hit;
                    //this will create 9 ray casts
                    for (float i = -4; i <= 4; i += .5f)
                    {

                        if (Physics.Raycast(start, transform.TransformDirection(Vector3.forward) + new Vector3(0, i / 8, 0), out hit, 3.5f) && PickUp == null)
                        {
                            //this checks if any of the rays hit an object with pickupables script
                            if (hit.collider.gameObject.GetComponent<PickUpAbles>() != null)
                            {
                                //if it does so all this
                                hit.collider.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                                hit.collider.gameObject.GetComponent<PickUpAbles>().UseGravity(true);
                                hit.collider.gameObject.GetComponent<PickUpAbles>().PlayerThatPickUpOBJ = this.gameObject;
                                PlayPickUpSound();
                                isHoldingOBJ = true;
                                hit.collider.gameObject.GetComponent<PickUpAbles>().IsPickedUped = true;
                                hit.collider.gameObject.GetComponent<PickUpAbles>().ChangeOwnerShip();
                                SetPickUpOBJ(hit.collider.gameObject);
                            }
                        }
                        //this is commented out just incase someone needs to look at the rays 
                        //Debug.DrawRay(start, transform.TransformDirection(Vector3.forward) + new Vector3(0, i, 0), Color.yellow);
                    }
                }//the else will instead of picking up the object will put down the obj
                else
                {

                    DropOBJ();
                    PickUp = null;
                    isHoldingOBJ = false;
                    PlayPickUpSound();
                }
                pickUpTimer = 1;
            }
            if (PickUp != null)
            {
                print(PickUp.name);
            }
            else
            {
                isPickingUpOBJ = false;
            }
            if (Input.GetButtonDown("Fire2") && PickUp != null)
            {
                ThrowOBJ(ThrowForce);
            }
            if (isHoldingOBJ == true)
            {
                Anim.SetBool("IsCarry", true);

            }
            else
            {
                if (Anim.GetCurrentAnimatorStateInfo(0).length >
                    Anim.GetCurrentAnimatorStateInfo(0).normalizedTime)
                {
                    Anim.SetBool("IsThrowing", false);
                }
                Anim.SetBool("IsCarry", false);
            }
        }
    }


    public void PlayPickUpSound()
    {
        player.PlaySound(PickUpSound); //play pickup sound
    }

    //you'll never guess what this func does 
    //no you really won't based off this name
    [PunRPC]
    public void DropPickUp()
    {
        PickUp.GetComponent<PickUpAbles>().IsPickedUped = false;
        PickUp = null;
        isPickingUpOBJ = false;
        isHoldingOBJ = false;

    }

    public void ThrowOBJ(int Force)
    {
        Anim.SetBool("IsThrowing", true);
        PlayPickUpSound();
        if (PickUp != null)
        {

            PickUp.GetComponent<Rigidbody>().AddForce(transform.forward * Force, ForceMode.Impulse);
            PickUp.GetComponent<PickUpAbles>().IsPickedUped = false;
            PickUp.GetComponent<PickUpAbles>().PlayerThatPickUpOBJ = null;
            isHoldingOBJ = false;
            PickUp = null;
        }
    }
    //or this one
    public void SetPickUpOBJ(GameObject OBJ)
    {
        PickUp = OBJ;
    }
    public void ResetPickUpPos()
    {
        if (PickUp != null)
        {
            PickUp.transform.position = transform.position + new Vector3(0, 2.5f, 0);
        }
    }
    public void DropOBJ()
    {
        if (PickUp != null)
        {
            PickUp.GetComponent<PickUpAbles>().DropPickUp();
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            //data that gets sent to other players
            stream.SendNext(isPickingUpOBJ);
            //stream.SendNext(PlayerThatPickUpOBJ);


        }
        else
        {
            //data recieved from other players
            isPickingUpOBJ = (bool)stream.ReceiveNext();
            //PlayerThatPickUpOBJ.transform.position = (Vector3)stream.ReceiveNext();


        }

    }


    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        //remove all objects player is carrying when the scene is switched
        if (eventCode == NetworkCodes.NetworkSceneChangedEventCode)
        {
            Debug.Log("Event Code: " + eventCode);
            DropOBJ();
            isPickingUpOBJ = false;
            PickUp = null;
            isHoldingOBJ = false;
        }
        else if (eventCode == NetworkCodes.DeleteObjectInDropoffCode)
        {
            int id = (int)((object[])photonEvent.CustomData)[0];
            // DestroyPickUp(id);
        }
    }
}
