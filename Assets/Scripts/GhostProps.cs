using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
public class GhostProps : MonoBehaviourPunCallbacks
{
    FindAllPickupables AllGMs;
    GameObject SpawnedGameobject;
    List<GameObject> GameObjectsApplicableWith = new List<GameObject>();
    GameObject Player;

    void Start()
    {
        //if this causes an error just put it on the gamemanager
        AllGMs = (FindAllPickupables)FindObjectOfType(typeof(FindAllPickupables));
    }

    
    void Update()
    {


        
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }


        
        if (photonView.IsMine)
        {
            if (GameObjectsApplicableWith.Count > 0)
            {
                if (GameObjectsApplicableWith.Count > 1)
                {
                    for (int i = 0; i < GameObjectsApplicableWith.Count; i++)
                    {
                        if (Vector3.Distance(this.gameObject.transform.position, GameObjectsApplicableWith[i].transform.position) < 3)
                        {
                            if (Player.GetComponent<PlayerPickUp>().PickUp == GameObjectsApplicableWith[i])
                            {
                                GameObjectsApplicableWith[i].GetComponent<PickUpAbles>().LockIntoPlace(this.transform);
                                Player.GetComponent<PlayerPickUp>().DropOBJ();
                                Player.GetComponent<Player>().score += 1;
                            }
                        }
                    }
                }
                else
                {
                    if (Vector3.Distance(this.gameObject.transform.position, GameObjectsApplicableWith[0].transform.position) < 3)
                    {
                        if (Player.GetComponent<PlayerPickUp>().PickUp == GameObjectsApplicableWith[0])
                        {
                            GameObjectsApplicableWith[0].GetComponent<PickUpAbles>().LockIntoPlace(this.transform);
                            Player.GetComponent<PlayerPickUp>().DropOBJ();
                            Player.GetComponent<Player>().score += 1;
                        }
                    }
                }
            } else
            {
                foreach (PickUpAbles G in AllGMs.AllGameObjects)
                {
                    if (G.name.Contains(gameObject.name))
                    {
                        GameObjectsApplicableWith.Add(G.gameObject);
                    }
                }
            }
        }
    }
}
