using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAbles : MonoBehaviour
{
    GameObject[] Player = new GameObject[4];
    void Awake()
    {
        Player = GameObject.FindGameObjectsWithTag("Player");
    }

    void Update()
    {
        
        for(int i = 0; i < Player.Length; i++)
        {
            if(Vector3.Distance(this.gameObject.transform.position,Player[i].transform.position) < 5)
            {
                if(Player[i].GetComponent<Player>().isHoldingOBJ == false && Player[i].GetComponent<Player>().isPickingUpOBJ == true)
                {
                    transform.parent = Player[i].transform;
                    transform.position = Player[i].transform.position + new Vector3(0, 1, 0);
                    Player[i].GetComponent<Player>().PickUp =  this.gameObject;
                    Player[i].GetComponent<Player>().isHoldingOBJ = true;
                }
            }
        }


    }
}
