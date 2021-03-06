using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Cinemachine;
using UnityEngine.UI;

namespace com.pebblz.finderskeepers
{
    public class Player : MonoBehaviourPunCallbacks, IPunObservable, IOnEventCallback
    {
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

        GameObject PauseScreen;

        [HideInInspector]
        public float StunCounter;

        public Transform mainCam;
        public GameObject freeLookCam;

        [HideInInspector]
        public PowerUp currentPowerUp;
        [HideInInspector]
        public float powerUpTimer = 0;
        [HideInInspector]
        public bool powerUpTimerActive = false;
        [HideInInspector]
        public Player_Movement pm;

        Animator Anim;

        CinemachineBrain camToHide;

        [HideInInspector]
        public Vector3 StartPosition = new Vector3(0, 1, 2);

        [HideInInspector]
        public bool isPaused;

        float pauseTimer;

        #region monobehaviour callbacks


        private void Awake()
        {

            soundManager = this.GetComponentInChildren<SoundManager>();
            camToHide = FindObjectOfType<CinemachineBrain>();
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

                //Finds current scene
                Scene scene = SceneManager.GetActiveScene();
                if (scene.name != "WinOrLose")
                {
                    if (PauseScreen == null)
                    {
                        PauseScreen = GameObject.FindGameObjectWithTag("Pause_Menu");
                        PauseScreen.SetActive(false);
                    }
                    if (Input.GetKeyDown(KeyCode.Escape) && pauseTimer <= 0)
                    {
                        isPaused = !isPaused;
                        pauseTimer = .5f;
                        setSliderValue();
                    }
                    if (isPaused)
                    {
                        PauseScreen.SetActive(true);
                        
                    }
                    else
                    {
                        PauseScreen.SetActive(false);
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


        private void setSliderValue()
        {
            GameObject[] sliders = GameObject.FindGameObjectsWithTag("SoundSlider");
            for(int i = 0; i < sliders.Length; i++)
            {
                sliders[i].GetComponent<ChangeVolume>().setVolumeLevel();
            }
        }

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
        
        public void ResetPosition()
        {
            if (GetComponent<PhotonView>().IsMine)
            {
                Anim.SetBool("Reset", true);
                Anim.SetBool("First", false);
                Anim.SetBool("Second", false);
                Anim.SetBool("Third", false);
                Anim.SetBool("Fourth", false);
                transform.position = StartPosition + new Vector3(Random.Range(0, 2), 1.5f, Random.Range(0, 2));
            }
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

                soundManager.PlayVictoryTheme();
                if (GetComponent<PhotonView>().IsMine)
                {
                    Anim.SetBool("Reset", false);
                    pm.enabled = false;
                }
                // find camera by the cinemachine brain instead so it does
                // not delete the camera in win or lose scene

                if (camToHide != null)
                {
                    camToHide.gameObject.SetActive(false);
                }
            }

            else if (eventCode == (byte)NetworkCodes.NetworkSceneChanged)
            {
                var playerPickUp = GetComponent<PlayerPickUp>();
                Debug.Log("Event Code: " + eventCode);
                playerPickUp.DropOBJ();
                playerPickUp.isPickingUpOBJ = false;
                playerPickUp.PickUp = null;
                playerPickUp.isHoldingOBJ = false;
            }
            else if (eventCode == (byte)NetworkCodes.ChangeToGameMusic)
            {
                soundManager.PlayGameTheme();
                //this gets called when we switch to the game scene
                this.GetComponent<PlayerTaser>().TasersLeft = 2;

            }
            else if (eventCode == (byte)NetworkCodes.ResetToLobby)
            {
                if (pm != null)
                    pm.enabled = true;
                ResetPosition();
                this.score = 0;
                this.GetComponent<PlayerTaser>().TasersLeft = 1_000_000;
                camToHide.gameObject.SetActive(true);
                soundManager.PlayLobbyTheme();
            }

        }
    }

}
