using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Photon.Pun;
using System.Linq;
using UnityEditor;

public class RoomRandomizer : MonoBehaviour/*, IOnEventCallback*/
{
    // arrays of spawnpoints and rooms respectively 
    public List<GameObject> roomSpawnpoints;
    public List<GameObject> rooms;
    GameObject temp
    //  private HashSet<int> roomIdxSpawned;
    [SerializeField] TodoList todolist;


    // Start is called before the first frame update
    void Start()
    {
        //rooms = Resources.LoadAll<GameObject>("Rooms").ToList();
        //rooms = new List<GameObject>();
        // rooms.Add(Resources.Load>("Rooms/"));

        //roomIdxSpawned = new HashSet<int>();
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnRooms();
            RandomizeRooms();
        }
    }

    void SpawnRooms()
    {

        // go through the spawnpoint array
        for (int i = 0; i < rooms.Count; i++)
        {
            temp = PhotonNetwork.Instantiate(rooms[i].name, roomSpawnpoints[i].transform.position, roomSpawnpoints[i].transform.rotation);
            PrefabUtility.UnpackPrefabInstance(temp.gameObject,PrefabUnpackMode.Completely,InteractionMode.AutomatedAction);
            temp.transform.DetachChildren();
        }

        if(todolist != null)
        {
            todolist.Active();
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
