using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{


    //List of the scripts that should only be active for the local
    //player (ex. PlayerController, MouseLook etc.)
    public MonoBehaviour[] localScripts;

    //List of the GameObjects that should only be active for the local 
    //player (ex. Camera, AudioListener etc.)
    public GameObject[] localObject;

    public static GameObject localInstance;

    public int score = 0;
    public bool isFiring = false;


    Rigidbody rb;
    public float StunCounter;



    public Transform mainCam;
    public GameObject freeLookCam;

    public PowerUp currentPowerUp;
    public float powerUpTimer = 0;
    public bool powerUpTimerActive = false;
    public Player_Movement pm;
    Animator Anim;

    #region monobehaviour callbacks


    private void Awake()
    {
        if (photonView.IsMine)
        {
            Anim = GetComponent<Animator>();
            gameObject.tag = "Player";
            Player.localInstance = gameObject;
            mainCam = GameObject.Find("Main Camera").GetComponent<Transform>();
            pm = GetComponent<Player_Movement>();
            DontDestroyOnLoad(mainCam);

        }
        else
        {
            freeLookCam.SetActive(false);
            GetComponent<AudioListener>().enabled = false;
        }
        DontDestroyOnLoad(this);





    }

    void Update()
    {
        if (powerUpTimerActive)
        {
            updatePowerUp();
        }

        if (GetComponent<PhotonView>().IsMine)
        {

            //Finds current scene
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name == "Main Game")
            {
                //if you're in game it'll lock your cursor and hide it 
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                //if you're not in game it'll unlock your cursor and make it visable
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            if (StunCounter > 0)
            {
                Anim.SetBool("IsStunned", true);
                Anim.SetBool("IsJumping", false);
                Anim.SetBool("IsCarry", false);
                Anim.SetBool("IsRunning", false);
            }
            else
            {
                Anim.SetBool("IsStunned", false);
            }
        }
    }

    #endregion




    public void StunPlayer()
    {

        StunCounter = 3;
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
            stream.SendNext(score);
        }
        else
        {
            this.score = (int)stream.ReceiveNext();
        }
    }
}
