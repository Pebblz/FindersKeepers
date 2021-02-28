﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player_Movement : MonoBehaviourPunCallbacks, IPunObservable
{
    /*Flower box
     * 
     * Edited by: Pat Naatz
     * 
     * Added walking sound
     * Added MoveToHere RPC
     */

    float speed = 5;

    public Transform mainCam;
    Animator Anim;
    Player ThisPlayer;
    public Rigidbody rb;
    float distToGround;
    float jumpspeed = 5;
    Vector3 moveDir;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    private SoundManager sfxManager;


    void Awake()
    {
        distToGround = GetComponent<Collider>().bounds.extents.y - .6f;
        ThisPlayer = GetComponent<Player>();
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        mainCam = GameObject.Find("Main Camera").GetComponent<Transform>();
        Anim = GetComponent<Animator>();
        sfxManager = this.GetComponentInChildren<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (photonView.IsMine)
        {
            #region Movement
            float H = Input.GetAxisRaw("Horizontal");
            float V = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(H, 0, V).normalized;

            if (direction.magnitude >= 0.1f)
            {
                if (ThisPlayer.StunCounter < 0)
                {
                    //sees how much is needed to rotate to match camera
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCam.localEulerAngles.y;

                    //used to smooth the angle needed to move to avoid snapping to directions
                    float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

                    //rotate player
                    transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

                    //converts rotation to direction / gives the direction you want to move in taking camera into account
                    moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                    rb.MovePosition(transform.position += moveDir.normalized * speed * Time.deltaTime);
                    Anim.SetBool("IsRunning", true);
                    sfxManager.PlayRunning();
                }
            }
            else
            {
                Anim.SetBool("IsRunning", false);
                sfxManager.StopRunningSFX();
                                                                          
            }
            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            {
                this.sfxManager.PlayJump();
                rb.velocity = new Vector3(direction.x, jumpspeed, direction.z);
                
            }

            if (IsGrounded())
            {
                Anim.SetBool("IsJumping", false);
            }
            else
            {
                Anim.SetBool("IsJumping", true);
                
            }
            #endregion
            ThisPlayer.StunCounter -= Time.deltaTime;
        }
    }
    public void setSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    public float getSpeed()
    {
        return speed;
    }
    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.01f);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

    /// <summary>
    /// Moves player to certain position
    /// Only the master client should call this functiton
    /// </summary>
    /// <param name="position"></param>
    [PunRPC]
    public void MoveToHere(Vector3 position)
    {
        transform.position = position;
    }
}