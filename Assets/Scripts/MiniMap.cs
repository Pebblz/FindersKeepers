using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    GameObject Player;


    // Update is called once per frame
    void Update()
    {
        if(Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }

        transform.position = new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z);

    }
}
