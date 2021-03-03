using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace com.pebblz.finderskeepers
{
    public class PlayerPickUp : MonoBehaviourPunCallbacks, IPunObservable
    {
        public GameObject PickUp;
        public bool isHoldingOBJ = false;
        public bool isPickingUpOBJ = false;
        [SerializeField]
        int ThrowForce;
        Animator Anim;
        float pickUpTimer;
        AudioSource Sound;
        [SerializeField]
        AudioClip PickUpSound;
        SoundManager sfxManager;

        // Start is called before the first frame update
        void Awake()
        {
            Anim = GetComponent<Animator>();
            Sound = GetComponent<AudioSource>();
            sfxManager = this.gameObject.GetComponentInChildren<SoundManager>();

        }


        // Update is called once per frame
        void Update()
        {

            pickUpTimer -= Time.deltaTime;

            if (photonView.IsMine)
            {

                //if the player holds q
                if (Input.GetButtonDown("Fire1") && pickUpTimer < 0 && !GetComponent<Player>().isPaused)
                {
                    //this checks to see if the players picking up an obj
                    if (PickUp == null)
                    {
                        isPickingUpOBJ = true;
                        Vector3 start = this.gameObject.transform.position + new Vector3(0, .5f, 0);
                        RaycastHit hit;
                        for (float x = -.5f; x <= .5f; x += .5f)
                        {
                            for (float y = -4; y <= 4; y += .6f)
                            {

                                if (Physics.Raycast(start, transform.TransformDirection(Vector3.forward) + new Vector3(x, y / 8, 0), out hit, 3.5f) && PickUp == null)
                                {
                                    //this checks if any of the rays hit an object with pickupables script
                                    if (hit.collider.gameObject.GetComponent<PickUpAbles>() != null)
                                    {
                                        //if it does so all this
                                        hit.collider.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                                        hit.collider.gameObject.GetComponent<PickUpAbles>().UseGravity(true);
                                        hit.collider.gameObject.GetComponent<PickUpAbles>().PlayerThatPickUpOBJ = this.gameObject;
                                        sfxManager.PlayPickUp();
                                        isHoldingOBJ = true;
                                        hit.collider.gameObject.GetComponent<PickUpAbles>().IsPickedUped = true;
                                        hit.collider.gameObject.GetComponent<PickUpAbles>().ChangeOwnerShip();
                                        SetPickUpOBJ(hit.collider.gameObject);
                                    }
                                }
                                else
                                {
                                    //this is commented out just incase someone needs to look at the rays 
                                    Debug.DrawRay(start, transform.TransformDirection(Vector3.forward) + new Vector3(x, y, 0), Color.yellow);
                                }
                            }
                        }
                    }//the else will instead of picking up the object will put down the obj
                    else
                    {

                        DropOBJ();
                        PickUp = null;
                        isHoldingOBJ = false;
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
                if (isHoldingOBJ)
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



        //you'll never guess what this func does 

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
            }
            else
            {
                //data recieved from other players
                isPickingUpOBJ = (bool)stream.ReceiveNext();
            }

        }

    }
}
