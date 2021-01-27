using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

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
        if(Input.GetKeyDown(KeyCode.Q))
        {
            isPickingUpOBJ = true;
        } else
        {
            isPickingUpOBJ = false;
        }

        float H = Input.GetAxisRaw("Horizontal");
        float V = Input.GetAxisRaw("Vertical");

        Vector3 tempVect = new Vector3(H, 0, V);
        tempVect = tempVect.normalized * speed * Time.deltaTime;
        rb.MovePosition(transform.position + tempVect);
    }
}
