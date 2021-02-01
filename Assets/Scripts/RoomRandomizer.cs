using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Photon.Pun;
using System.Linq;
using UnityEditor;

public class RoomRandomizer : MonoBehaviourPunCallbacks, IPunObservable
{
    // arrays of spawnpoints and rooms respectively 
    public List<GameObject> roomSpawnpoints;
    public List<GameObject> rooms;
    //  private HashSet<int> roomIdxSpawned;
    [SerializeField] TodoList todolist;
    public bool hardMode = false;


    // Start is called before the first frame update
    void Start()
    {
        //rooms = Resources.LoadAll<GameObject>("Rooms").ToList();
        //rooms = new List<GameObject>();
        // rooms.Add(Resources.Load>("Rooms/"));

        //roomIdxSpawned = new HashSet<int>();
        if (PhotonNetwork.IsMasterClient)
        {
            if (hardMode)
            {
                SpawnRoomsHardMode();
            }
            else
            {
                SpawnRooms();
            }
        }
    }

    void SpawnRoomsHardMode()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            GameObject g = rooms[i];
            
            int rng = Random.Range(0, roomSpawnpoints.Count);
            
            g.transform.position = roomSpawnpoints[rng].transform.position;
            g.transform.rotation = roomSpawnpoints[rng].transform.rotation;
            roomSpawnpoints.Remove(roomSpawnpoints[rng]);
         
            string[] n = g.name.Split('(');
            Debug.Log(n[0]);

            g.name = n[0];

            PhotonNetwork.Instantiate(g.name, g.transform.position, g.transform.rotation);
        }

        if(todolist != null)
        {        
                todolist.Active();           
        }
    }

    void SpawnRooms()
    {

        // go through the spawnpoint array
        for (int i = 0; i < rooms.Count; i++)
        {
            PhotonNetwork.Instantiate(rooms[i].name, roomSpawnpoints[i].transform.position, roomSpawnpoints[i].transform.rotation);
        }

        if (todolist != null)
        {
            todolist.Active();
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(hardMode);
        }
        else
        {
            hardMode = (bool)stream.ReceiveNext();
        }
    }
}
