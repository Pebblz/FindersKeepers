using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Photon.Pun;
using System.Linq;

public class RoomRandomizer : MonoBehaviour/*, IOnEventCallback*/
{
    // arrays of spawnpoints and rooms respectively 
    public List<GameObject> roomSpawnpoints;
    public List<GameObject> rooms;

  //  private HashSet<int> roomIdxSpawned;

    

    // Start is called before the first frame update
    void Start()
    {
        //rooms = Resources.LoadAll<GameObject>("Rooms").ToList();
        //rooms = new List<GameObject>();
        // rooms.Add(Resources.Load>("Rooms/"));

        //roomIdxSpawned = new HashSet<int>();
        SpawnRooms();
        RandomizeRooms();
    }

    void SpawnRooms()
    {
        if (PhotonNetwork.IsMasterClient)
        {
                // go through the spawnpoint array
                for (int i = 0; i < rooms.Count; i++)
            {
                PhotonNetwork.Instantiate(rooms[i].name, roomSpawnpoints[i].transform.position, roomSpawnpoints[i].transform.rotation);
            }
        }

    }
    void RandomizeRooms()
    {
      
            for (int i = 0; i < rooms.Count; i++)
            {
                int rng = Random.Range(0, roomSpawnpoints.Count);

                rooms[i].transform.position = roomSpawnpoints[rng].transform.position;
                rooms[i].transform.rotation = roomSpawnpoints[rng].transform.rotation;
                roomSpawnpoints.Remove(roomSpawnpoints[rng]);
            }
    }
   
}
