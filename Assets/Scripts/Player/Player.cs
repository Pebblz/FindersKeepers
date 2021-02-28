using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Cinemachine;

public class Player : MonoBehaviourPunCallbacks, IPunObservable, IOnEventCallback
{
    /*Fb
     * 
     * Edited By: Patrick Naatz
     * Added:
     *    the WinOrLoseSceneEvent functionality into the onevent function
     */

    //List of the scripts that should only be active for the local
    //player (ex. PlayerController, MouseLook etc.)
    [SerializeField]
    MonoBehaviour[] localScripts;

    //List of the GameObjects that should only be active for the local 
    //player (ex. Camera, AudioListener etc.)
    public GameObject[] localObject;

    public static GameObject localInstance;

    public int score = 0;
    [HideInInspector]
    public bool isFiring = false;
    SoundManager soundManager;

    Rigidbody rb;
    public float StunCounter;


    
    public Transform mainCam;
    public GameObject freeLookCam;

    public PowerUp currentPowerUp;
    public float powerUpTimer = 0;
    public bool powerUpTimerActive = false;
    public Player_Movement pm;
    Animator Anim;

    [HideInInspector]
    public Vector3 StartPosition;

    [HideInInspector]
    public bool isPaused;
    
    float pauseTimer;

    #region monobehaviour callbacks


    private void Awake()
    {
        soundManager  = this.GetComponentInChildren<SoundManager>();
        if (photonView.IsMine)
        {
            StartPosition = transform.position;
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
            soundManager.isRemotePlayer = true;
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
            if(Input.GetKeyDown(KeyCode.Escape) && pauseTimer <= 0)
            {
                isPaused = !isPaused;
                pauseTimer = 1f;
            }
            //Finds current scene
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name == "Main Game")
            {
                if (isPaused)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    //if you're in game it'll lock your cursor and hide it 
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
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
            pauseTimer -= Time.deltaTime;
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
    public void ResetPosition()
    {
        transform.position = StartPosition;
        GetComponent<PlayerMovement>().enabled = true;
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

    /// <summary>
    /// If you're not photon network you should not be calling this function
    /// </summary>
    /// <param name="photonEvent"></param>
    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        //remove all objects player is carrying when the scene is switched
        if (eventCode == (byte)NetworkCodes.SwitchToWinOrLoseScene)
        {
            Debug.Log("Event Code: " + eventCode);
            GetComponent<Player_Movement>().enabled = false;
            
            // find camera by the cinemachine brain instead so it does
            // not delete the camera in win or lose scene
            var camToDestroy = FindObjectOfType<CinemachineBrain>();
            if(camToDestroy != null)
            {
                Destroy(camToDestroy.gameObject);
            }
        }
    }
}
