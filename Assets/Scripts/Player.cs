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
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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

        Vector3 tempVect = new Vector3(H, 0, V);
        tempVect = tempVect.normalized * speed * Time.deltaTime;
        if (StunCounter <= 0)
        {
            rb.MovePosition(transform.position + tempVect);
        }
        else
        {
            StunCounter -= Time.deltaTime;
        }
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
