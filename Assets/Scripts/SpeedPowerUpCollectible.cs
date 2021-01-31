using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class SpeedPowerUpCollectible : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameObject Instance = null;
    public PowerUp powerUp;
    public AudioClip pickUpNoise;
    public PowerUpSpawner pc;
    public bool collided = false;

    public void Awake()
    {
        powerUp = new SpeedPowerUp();
        //gameObject.tag = "PowerUpCollectible";
        SpeedPowerUpCollectible.Instance = gameObject;
        pc = GameObject.Find("powerUpSpawner").GetComponent<PowerUpSpawner>();
    }

    public void Update()
    {
        if (photonView.IsMine)
        {
            if (collided)
            {
                pc.removeFromList();
                photonView.RPC("DestroyGlobally", RpcTarget.All);

                //DestroyGlobally();
            }
        }
        //else if (!PhotonNetwork.IsMasterClient)
        //{
        //    if (collided)
        //    {
        //        PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
        //        PhotonNetwork.Destroy(this.gameObject);
        //        //photonView.RPC("DestroyGlobally", RpcTarget.All);

        //    }
        //}

    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player" && !collided)
        {
            if (collision.collider.gameObject.GetComponent<Player>().getPowerUp() != null)
            {
                collision.collider.gameObject.GetComponent<Player>().deactivatePowerUp();
            }
            collision.collider.gameObject.GetComponent<Player>().setPowerUp(powerUp);
            collision.collider.gameObject.GetComponent<Player>().activatePowerUp();

            collided = true;

            if (photonView.IsMine)
            {
                //    pc.removeFromList();
                //    //photonView.RPC("DestroyGlobally", RpcTarget.All);

                //    DestroyGlobally();
                //}
                //else
                //{
                //    photonView.TransferOwnership(PhotonNetwork.MasterClient);
                //    PhotonNetwork.Destroy(this.gameObject);
                //}
                //// }
                // else
                // {
                
            }
            if(collided)
            {
                DestroyGlobally();
            }
            // // collided = true;
        }
    }

    [PunRPC]

    public void DestroyGlobally()
    {
        //if(!PhotonNetwork.IsMasterClient)
        //{
        //  photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        //}
        //PhotonNetwork.Destroy(this.gameObject);
        if (GetComponent<PhotonView>().Owner == PhotonNetwork.LocalPlayer)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        else
        {
            GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(collided);
        }
        else
        {
            collided = (bool)stream.ReceiveNext();
        }
    }
}
