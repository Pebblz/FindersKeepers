using UnityEngine;
using Photon.Pun;
namespace com.pebblz.finderskeepers
{
    public class PickUpableSpawner : MonoBehaviourPunCallbacks
    {
        GameObject[] CurrentlySpawnedOBJ = new GameObject[99];

        public void FindOBJ()
        {
            CurrentlySpawnedOBJ = GameObject.FindGameObjectsWithTag("PointsPickUp");
        }
        //This will be for when we need to destroy a GameOBJ for all players
        [PunRPC]
        public void deleteOBJ(GameObject ObjectToDelete)
        {
            //it works 
            if (ObjectToDelete.GetComponent<PhotonView>().Owner == PhotonNetwork.LocalPlayer)
            {
                PhotonNetwork.Destroy(ObjectToDelete);
            }
            else
            {
                ObjectToDelete.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
                PhotonNetwork.Destroy(ObjectToDelete);
            }

        }
        //This'll be for if we want to reset the house when we start a new round
        [PunRPC]
        public void deleteAllOBJ()
        {
            //this loops through all the currently spawned objs and destroys them
            for (int i = 0; i < CurrentlySpawnedOBJ.Length; i++)
            {

                if (CurrentlySpawnedOBJ[i].GetComponent<PhotonView>().Owner == PhotonNetwork.LocalPlayer)
                {
                    PhotonNetwork.Destroy(CurrentlySpawnedOBJ[i]);
                }
                else
                {
                    CurrentlySpawnedOBJ[i].GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
                    PhotonNetwork.Destroy(CurrentlySpawnedOBJ[i]);
                }
            }
        }

    }
}
