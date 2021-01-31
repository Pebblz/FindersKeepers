using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using ExitGames.Client.Photon;

[RequireComponent(typeof(AudioSource))]
public class PlayerPickUp : MonoBehaviourPunCallbacks, IPunObservable, IOnEventCallback
{
    /*Flower Box
     * 
     * Edited By: Pat Naatz
     * 
     * Added Pick Up Sound
     */


    public GameObject PickUp;
    public bool isHoldingOBJ = false;
    public bool isPickingUpOBJ = false;
    [SerializeField]
    int ThrowForce;
    Animator Anim;
    float pickUpTimer;


    AudioSource Sound;
    [SerializeField] AudioClip PickUpSound;

    // Start is called before the first frame update
    void Awake()
    {
        Anim = GetComponent<Animator>();
        Sound = GetComponent<AudioSource>();
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
                if (PickUp == null)
                {
                    isPickingUpOBJ = true;
                }
                else
                {
                    DropOBJ();
                    PickUp = null;
                    isHoldingOBJ = false;
                }
                pickUpTimer = 1;
            }
            else
            {
                isPickingUpOBJ = false;
            }
            if(Input.GetButtonDown("Fire2") && PickUp != null)
            {
                ThrowOBJ(ThrowForce);
            }
            if (isHoldingOBJ == true)
            {
                Anim.SetBool("IsCarry", true);
                Sound.PlayOneShot(PickUpSound); //play pickup sound
            }
            else
            {
                Anim.SetBool("IsCarry", false);
            }
        }
    }
    //you'll never guess what this func does 
    //no you really won't based off this name
    public void DestroyPickUp()
    {
        if (PickUp != null)
        {
            PickUp.GetComponent<Rigidbody>().useGravity = false;
            PickUp.GetComponent<BoxCollider>().enabled = false;
            PickUp.transform.position = new Vector3(0, -40, 0);
        }
    }
    public void ThrowOBJ(int Force)
    {
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
        } else if (eventCode == NetworkCodes.DeleteObjectInDropoffCode)
        {
            
         
        }

       
    }
}
