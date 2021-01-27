using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOffZoneScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider c)
    {
        if(c.tag == "Player")
        {
           if( c.GetComponent<Player>().isHoldingOBJ == true)
            {
                c.GetComponent<Player>().score += 1;
                c.GetComponent<Player>().DestroyPickUp();
                c.GetComponent<Player>().isHoldingOBJ = false;
            }
        }
    }

}
