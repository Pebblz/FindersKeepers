using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PickUpableSpawner : MonoBehaviourPunCallbacks
{
    GameObject[] PickablesToSpawn = new GameObject[99];
    GameObject[] PickablesSpawnLocation = new GameObject[99];
    GameObject[] CurrentlySpawnedOBJ = new GameObject[99];
    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "PickUpSpawner";

        //this is here so when we randomize the rooms we can just have the 
        //empty gameobjects for where they will go in the room prefab
        PickablesToSpawn = GameObject.FindGameObjectsWithTag("LocationForPickUp");
        SpawnOBJ();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SpawnOBJ()
    {
        for(int i = 0; i < PickablesToSpawn.Length; i++)
        {
            //this will loop through all the gameobjs that need to be spawned and spawn it at the given pos
            if (PickablesToSpawn[i] != null)
            {
                //this'll add a new currently spawned obj to the array whenever a new obj gets created 
                CurrentlySpawnedOBJ[i] = PhotonNetwork.Instantiate(PickablesToSpawn[i].name,
                    PickablesSpawnLocation[i].transform.position, Quaternion.identity);
            }
        }
    }

    //This will be for when we need to destroy a GameOBJ for all players
    [PunRPC]
    void deleteOBJ(GameObject ObjectToDelete)
    {
        PhotonNetwork.Destroy(ObjectToDelete);
    }
    //This'll be for if we want to reset the house when we start a new round
    [PunRPC]
    void deleteAllOBJ()
    {
        //this loops through all the currently spawned objs and 
        //SHOULD delete all the obj's for all players 
        for(int i = 0; i < CurrentlySpawnedOBJ.Length; i++)
        {
            PhotonNetwork.Destroy(CurrentlySpawnedOBJ[i]);
        }
    }
}
