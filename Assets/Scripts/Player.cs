using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    GameObject PickUp;

    public int score = 0;
    int TasersLeft = 30;
    Rigidbody rb;
    float StunCounter;
    [SerializeField]
    float speed;
    [SerializeField]
    GameObject taserOBJ;
    public bool isHoldingOBJ;
    public bool isPickingUpOBJ;
    public Transform mainCam;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCam = GameObject.Find("Main Camera").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
        //if the player holds q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isPickingUpOBJ = true;
        }
        else
        {
            isPickingUpOBJ = false;
        }
        if (Input.GetKeyDown(KeyCode.Space) && TasersLeft > 0)
        {

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
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
     
                rb.MovePosition(transform.position += moveDir.normalized * speed * Time.deltaTime);
            }
            else
            {
                StunCounter -= Time.deltaTime;
            }
        }

       // Vector3 tempVect = new Vector3(H, 0, V);
       // tempVect = tempVect.normalized * speed * Time.deltaTime;
       
        #endregion
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
    }
    public void StunPlayer()
    {
        StunCounter = 3;
    }
}
