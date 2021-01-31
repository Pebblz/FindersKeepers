using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Photon.Pun;

public class RoomRandomizer : MonoBehaviour/*, IOnEventCallback*/
{
    // arrays of spawnpoints and rooms respectively 
    public List<GameObject> roomSpawnpoints;
    public List<GameObject> rooms;

    // spawnpoint and room duh
    public GameObject spawnpoint;
    public GameObject room;

    private HashSet<int> numbersInThing;

    

    // Start is called before the first frame update
    void Awake()
    {
        roomSpawnpoints = new List<GameObject>();
        rooms = new List<GameObject>();
        //// finding the game objects with the right tags
        //roomSpawn.points = GameObject.FindGameObjectsWithTag("RoomSpawn");
        //rooms = GameObject.FindGameObjectsWithTag("Room");

        numbersInThing = new HashSet<int>();
        spawnRooms();
    }

    //void RandomizeRoom()
    //{
    //    // go through the spawnpoint array
    //    for(int i = 0; i < roomSpawnpoints.Count - 1; i++)
    //    {
    //        int index = Random.Range(0, 11);

    //        while (numbersInThing.Contains(index))
    //        {
    //            index = Random.Range(0, 11);
    //        }

    //        numbersInThing.Add(index);

    //        spawnpoint = roomSpawnpoints[i];
    //        //pick a random room and place it at the spawnpoints location and rotation (rotation might be a little funky)
    //        room = rooms[index];
    //        room.transform.position = spawnpoint.transform.position;
    //        room.transform.rotation = spawnpoint.transform.rotation;
    //    }

    //}

    void spawnRooms()
    {
        for(int i = 0; i < rooms.Count; i++)
        {
            int rng = Random.Range(0, roomSpawnpoints.Count);

            PhotonNetwork.Instantiate(rooms[i].name, roomSpawnpoints[rng].transform.position, roomSpawnpoints[rng].transform.rotation);                
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Room")
        {
            // if the room collides with another room, re randomie
          // RandomizeRoom();
        }
    }

    //public void OnEvent(EventData photonEvent)
    //{
    //    byte eventCode = photonEvent.Code;
    //    Debug.Log("kokmongus");
    //    if (eventCode == NetworkCodes.RandomRoomEventCode)
    //    {
    //        RandomizeRoom();
            
    //    }
    //}
}
