using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player_Movement : MonoBehaviourPunCallbacks, IPunObservable
{


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
    void Awake()
    {
        distToGround = GetComponent<Collider>().bounds.extents.y;
        ThisPlayer = GetComponent<Player>();
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        mainCam = GameObject.Find("Main Camera").GetComponent<Transform>();
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            Debug.Log("a");
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
                if (ThisPlayer.StunCounter <= 0)
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
  
                }
                else
                {
                    ThisPlayer.StunCounter -= Time.deltaTime;

                }
            }
            else
            {
                Anim.SetBool("IsRunning", false);
            }
            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            {
                rb.velocity = new Vector3(direction.x, jumpspeed, direction.z);
            }
        #endregion

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
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
