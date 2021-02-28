using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace com.pebblz.finderskeepers
{
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
        }
        public void Start()
        {
        }

        public void Update()
        {
            if (photonView.IsMine)
            {
                if (collided)
                {
                    pc = GameObject.Find("powerUpSpawner").GetComponent<PowerUpSpawner>();
                    pc.removeFromList();
                    PhotonNetwork.Destroy(gameObject);
                }
            }
            if (collided)
            {
                photonView.RPC("DestroyGlobally", RpcTarget.All);
            }

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
            }
        }

        [PunRPC]

        public void DestroyGlobally()
        {
            pc = GameObject.Find("powerUpSpawner").GetComponent<PowerUpSpawner>();
            pc.removeFromList();
            Destroy(this.gameObject);
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
}
