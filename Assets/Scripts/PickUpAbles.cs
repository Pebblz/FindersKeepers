using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAbles : MonoBehaviour
{
    //an [] of Player objs
    GameObject[] Player = new GameObject[4];

    bool IsPickedUped;
    GameObject PlayerThatPickUpOBJ;
    //this is an awake because it'll do this whenever this object gets spawned
    void Awake()
    {
        //finds the players
        Player = GameObject.FindGameObjectsWithTag("Player");
    }

    void Update()
    {
        
        for(int i = 0; i < Player.Length; i++)
        {
            //checks how close the players are to the obj 
            if(Vector3.Distance(this.gameObject.transform.position,Player[i].transform.position) < 5)
            {
                //if he's getting ready to pick up the obj
                if(Player[i].GetComponent<Player>().isHoldingOBJ == false && 
                    Player[i].GetComponent<Player>().isPickingUpOBJ == true && IsPickedUped == false) 
                {
                    //does this stuff
                    transform.parent = Player[i].transform;
                    transform.position = Player[i].transform.position + new Vector3(0, 1, 0);
                    Player[i].GetComponent<Player>().SetPickUpOBJ(this.gameObject);
                    Player[i].GetComponent<Player>().isHoldingOBJ = true;
                    PlayerThatPickUpOBJ = Player[i];
                    //Object.FindObjectOfType<TodoList>().PickUpObject(this); //tells the list it was picked up
                    IsPickedUped = true;
                }
            }
        }


    }
}
