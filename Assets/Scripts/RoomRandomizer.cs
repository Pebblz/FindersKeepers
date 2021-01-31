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

    private HashSet<int> roomIdxSpawned;

    

    // Start is called before the first frame update
    void Awake()
    {
        roomSpawnpoints = Resources.LoadAll<GameObject>("RoomSpawnLocations").ToList();
        //rooms = Resources.LoadAll<GameObject>("Rooms").ToList();
        //rooms = new List<GameObject>();
        // rooms.Add(Resources.Load>("Rooms/"));

        roomIdxSpawned = new HashSet<int>();
        RandomizeRoom();
    }

    void RandomizeRoom()
    {
        // go through the spawnpoint array
        for (int i = 0; i < roomSpawnpoints.Count; i++)
        {
            int index = Random.Range(0, rooms.Count);

            while (roomIdxSpawned.Contains(index))
            {
                index = Random.Range(0, rooms.Count);
            }

            roomIdxSpawned.Add(index);
            string name = rooms[index].name;
            Vector3 pos = roomSpawnpoints[index].transform.position;
            Quaternion rot = roomSpawnpoints[index].transform.rotation;

            PhotonNetwork.Instantiate(name, pos, rot);

        }

    }
   
}
