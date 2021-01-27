using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOffZoneScript : MonoBehaviour
{
    //so guess what this little thing does 
    //i'll give you a sec to figure it out 
    //so what it does is if you enter the trigger it does the stuff 
    private void OnTriggerEnter(Collider c)
    {
        //if a Player enters the room
        if(c.tag == "Player")
        {
            //then it makes sure to see if the players holding an object
           if( c.GetComponent<Player>().isHoldingOBJ == true)
            {
                //then it'll encroment the score by 1 
                c.GetComponent<Player>().score += 1;
                //destroy the pickuped obj
                c.GetComponent<Player>().DestroyPickUp();
                //and set his holding obj to false
                c.GetComponent<Player>().isHoldingOBJ = false;
            }
        }
    }

}
