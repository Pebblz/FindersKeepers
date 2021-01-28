using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField]
    public GameObject PickUp;


    public static GameObject localInstance;
    
    public int score = 0;
    public bool isFiring = false;

    int TasersLeft = 30;
    Rigidbody rb;
    float StunCounter;
    [SerializeField]
    float speed = 5;
    [SerializeField]
    GameObject taserOBJ;

    public bool isHoldingOBJ;
    public bool isPickingUpOBJ;
    public Transform mainCam;
    public GameObject freeLookCam;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    Vector3 moveDir;
    float distToGround;
    float jumpspeed = 5;
    float pickUpTimer;

    public PowerUp currentPowerUp;
    public float powerUpTimer = 0;
    public bool powerUpTimerActive = false;


    #region monobehaviour callbacks


    private void Awake()
    {
        if (photonView.IsMine)
        {
            Player.localInstance = this.gameObject;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        distToGround = GetComponent<Collider>().bounds.extents.y;
        mainCam = GameObject.Find("Main Camera").GetComponent<Transform>();
        freeLookCam = GameObject.Find("FreeLookCam");
    }

    // Update is called once per frame
    void Update()
    {
        pickUpTimer -= Time.deltaTime;
        //if the player holds q
        if (Input.GetKeyDown(KeyCode.Q) && pickUpTimer <= 0)
        {
            if (PickUp == null)
            {
                isPickingUpOBJ = true;
            } else
            {
                PickUp.GetComponent<PickUpAbles>().DropPickUp();
                PickUp = null;
                isHoldingOBJ = false;
            }
            pickUpTimer = 1;
        }
        else
        {
            isPickingUpOBJ = false;
        }
        if (Input.GetKeyDown(KeyCode.E) && TasersLeft > 0)
        {
            isFiring = true;
            shootTaser();
        }

        #region Movement
        float H = Input.GetAxisRaw("Horizontal");
        float V = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(H, 0, V).normalized;

        if (direction.magnitude >= 0.1f)
        {
            if (StunCounter <= 0)
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
            }
            else
            {
                StunCounter -= Time.deltaTime;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.velocity = new Vector3(direction.x, jumpspeed, direction.z);
        }
        #endregion

        if (powerUpTimerActive)
        {
            updatePowerUp();
        }
    }

    #endregion

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }
    //you'll never guess what this func does 
    public void DestroyPickUp()
    {
        if (PickUp != null)
            Destroy(PickUp);
    }
    //or this one
    public void SetPickUpOBJ(GameObject OBJ)
    {
        PickUp = OBJ;
    }
    void shootTaser()
    {

        //this spawns the bullet 
        GameObject temp = Instantiate(taserOBJ, this.gameObject.transform.position, Quaternion.identity);
        //this makes sure player doesn't shoot himself 
        temp.GetComponent<Taser>().PlayerWhoShotThis = gameObject;
        //bullet go forward
        temp.GetComponent<Rigidbody>().velocity = transform.forward * 10f;
        //you lose a taser if you shoot a taser
        TasersLeft -= 1;
        isFiring = false;
    }
    public void StunPlayer()
    {
        StunCounter = 3;
    }
    public void setSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    public float getSpeed()
    {
        return speed;
    }

    public void setPowerUp(PowerUp newPowerUp)
    {
        currentPowerUp = newPowerUp;
    }

    public PowerUp getPowerUp()
    {
        return currentPowerUp;
    }

    public void activatePowerUp()
    {
        powerUpTimerActive = true;
        powerUpTimer = 20 % 60;
    }

    public void deactivatePowerUp()
    {
        powerUpTimerActive = false;
        currentPowerUp.deactivate(this);
        powerUpTimer = 0;
    }
    public void updatePowerUp()
    {
        if (powerUpTimer > 0)
        {
            currentPowerUp.activate(this);
            powerUpTimer -= Time.deltaTime;
        }
        else
        {
            deactivatePowerUp();
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {


        if (stream.IsWriting)
        {
            //data that gets sent to other players
            stream.SendNext(isFiring);
            stream.SendNext(score);
            stream.SendNext(isHoldingOBJ);
            stream.SendNext(PickUp);
        } else
        {
            //data recieved from other players
            this.isFiring = (bool)stream.ReceiveNext();
            this.score = (int)stream.ReceiveNext();
            this.isHoldingOBJ = (bool)stream.ReceiveNext();
            this.PickUp = (this.isHoldingOBJ) ? (GameObject) stream.ReceiveNext() : null;

        }
    } 
}
