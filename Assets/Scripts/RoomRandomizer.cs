using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomRandomizer : MonoBehaviour
{

    public float roomNumber;
    //this is bad code this is bad code this is bad code these variables are cringe
    public GameObject spawnpoint;
    public GameObject spawnpoint2;
    public GameObject spawnpoint3;
    public GameObject room;

    // Start is called before the first frame update
    void Start()
    {
       roomNumber =  Random.Range(0, 3);
        room = this.gameObject;
        RandomizeRoom();
        // spawnpoint = GameObject.FindGameObjectWithTag("RoomSpawn");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RandomizeRoom()
    {
        roomNumber = Random.Range(0, 3);

        switch (roomNumber)
        {
            case 0:
                room.transform.position = spawnpoint.transform.position;
                room.transform.rotation = spawnpoint.transform.rotation;
                break;
            case 1:
                room.transform.position = spawnpoint2.transform.position;
                room.transform.rotation = spawnpoint2.transform.rotation;
                break;
            case 2:
                room.transform.position = spawnpoint3.transform.position;
                room.transform.rotation = spawnpoint3.transform.rotation;
                break;
            case 3:
                //Debug.Log("3");
                break;
            case 4:
                //Debug.Log("4");
                break;
            case 5:
                // Debug.Log("5");
                break;
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Room")
        {
            RandomizeRoom();
        }

        
    }
}
