using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class SpeedPowerUpCollectible : MonoBehaviourPunCallbacks, IPunObservable
{
    public PowerUp powerUp;
    public AudioClip pickUpNoise;
    public bool collided = false;

    public void Awake()
    {
        powerUp = new SpeedPowerUp();
    }

    public void Update()
    {

        //if (photonView.IsMine)
        //{
        //    if (collided)
        //    {
        //        PhotonNetwork.Destroy(this.gameObject);
        //    }
        //}
        //else
        //{
        //    photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        //    if (collided)
        //    {
        //        PhotonNetwork.Destroy(this.gameObject);
        //    }
        //}
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (!photonView.IsMine)
        {
            Debug.Log("o");
            return;
        }

        if (collision.collider.tag == "Player" && !collided)
        {
            if (collision.collider.gameObject.GetComponent<Player>().getPowerUp() != null)
            {
                collision.collider.gameObject.GetComponent<Player>().deactivatePowerUp();
            }
            collision.collider.gameObject.GetComponent<Player>().setPowerUp(powerUp);
            collision.collider.gameObject.GetComponent<Player>().activatePowerUp();

            if(photonView.IsMine)
            {
                Destroy(gameObject);
                gameObject.GetComponent<PhotonView>().TransferOwnership(this.gameObject.GetComponent<PhotonView>().Owner);
                gameObject.GetComponent<PhotonView>().RPC("DestroyGlobally", RpcTarget.All);
            }          
           // collided = true;
        }        
    }

    [PunRPC]

    public void DestroyGlobally()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //if(stream.IsWriting)
        //{
        //    stream.SendNext(collided);
        //}
        //else
        //{
        //    collided = (bool)stream.ReceiveNext();
        //}
    }
}
