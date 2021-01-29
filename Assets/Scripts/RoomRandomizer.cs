﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomRandomizer : MonoBehaviour
{
    // arrays of spawnpoints and rooms respectively 
    public GameObject[] roomSpawnpoints;
    public GameObject[] rooms;

    // spawnpoint and room duh
    public GameObject spawnpoint;
    public GameObject room;

    // Start is called before the first frame update
    void Start()
    {
        // finding the game objects with the right tags
        roomSpawnpoints = GameObject.FindGameObjectsWithTag("RoomSpawn");
        rooms = GameObject.FindGameObjectsWithTag("Room");
        RandomizeRoom();
    }

    // Update is called once per frame
    void Update()
    {
        // woooooosh (because its empty)
    }

    void RandomizeRoom()
    {
        // go through the spawnpoint array
        for(int i = 0; i < roomSpawnpoints.Length; i++)
        {
            spawnpoint = roomSpawnpoints[i];
            //pick a random room and place it at the spawnpoints location and rotation (rotation might be a little funky)
            room = rooms[Random.Range(0, 11)];
            room.transform.position = spawnpoint.transform.position;
            room.transform.rotation = spawnpoint.transform.rotation;
        }

    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Room")
        {
            // if the room collides with another room, re randomie
            RandomizeRoom();
        }
    }
}
