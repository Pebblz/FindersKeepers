using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

namespace com.pebblz.finderskeepers
{
    public class PlayerTaser : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        GameObject taserOBJ;
        //you need to keep a instance of the prefab to be able to delete it
        //and this is that instance
        public GameObject taserInstance;
        
        public int TasersLeft = 2;
        Animator Anim;
        void Awake()
        {
            Anim = GetComponent<Animator>();
        }

        void Update()
        {
            if (photonView.IsMine)
            {
                if (Input.GetKeyDown(KeyCode.E) && TasersLeft > 0 && !GetComponent<Player>().isPaused)
                {
                    if (!GetComponent<PlayerPickUp>().isHoldingOBJ)
                    {
                        GetComponent<Player>().isFiring = true;
                        shootTaser();
                        Anim.SetBool("IsShooting", true);
                    }
                }
                if (Anim.GetCurrentAnimatorStateInfo(0).IsName("Shooting"))
                {
                    Anim.SetBool("IsShooting", false);
                }
            }
        }
        void shootTaser()
        {

            //this spawns the bullet 
            taserInstance = PhotonNetwork.Instantiate("Test_Taser", this.gameObject.transform.position + new Vector3(0, 1.2f, 0), Quaternion.identity);
            //this makes sure player doesn't shoot himself 
            taserInstance.GetComponent<Taser>().PlayerWhoShotThis = gameObject;
            //bullet go forward
            taserInstance.GetComponent<Rigidbody>().velocity = transform.forward * 10f;
            //you lose a taser if you shoot a taser
            TasersLeft -= 1;
            GetComponent<Player>().isFiring = false;

        }

    }
}
