using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    GameObject PickUp;

    public int score = 0;

    Rigidbody rb;

    [SerializeField]
    float speed;

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
        if(Input.GetKeyDown(KeyCode.Q))
        {
            isPickingUpOBJ = true;
        } else
        {
            isPickingUpOBJ = false;
        }
        #region Movement
        float H = Input.GetAxisRaw("Horizontal");
        float V = Input.GetAxisRaw("Vertical");

        Vector3 tempVect = new Vector3(H, 0, V);
        tempVect = tempVect.normalized * speed * Time.deltaTime;
        rb.MovePosition(transform.position + tempVect);
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
}
