using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace com.pebblz.finderskeepers
{
    public class PowerUpSpawner : MonoBehaviourPunCallbacks, IPunObservable
    {
        public static GameObject Instance = null;
        public float timer = 2 % 60;
        public int maxNumOfPowerUps = 1;
        public int currentNumOfPowerups = 0;

        // Start is called before the first frame update
        void Awake()
        {
            gameObject.tag = "PowerUpSpawner";
            PowerUpSpawner.Instance = gameObject;
        }

        // Update is called once per frame
        void Update()
        {
            if (timer <= 0)
            {
                timer = 0;
            }
            else
            {
                timer -= Time.deltaTime;
            }
            if (currentNumOfPowerups < maxNumOfPowerUps && timer <= 0)
            {
                timer = 2 % 60;

                if (photonView.IsMine)
                {
                    SpawnPowerUp();
                }
            }
        }

        [PunRPC]
        public void SpawnPowerUp()
        {
            Vector3 pos = new Vector3(0, 0, Random.Range(0, 6));
            int rng = Random.Range(0, 2);

            switch (rng)
            {
                case 0:
                    {
                        currentNumOfPowerups++;
                        //powerUps.Add(Resources.Load<PowerUpCollectible>("SpeedPowerUp"));
                        PhotonNetwork.Instantiate("SpeedPowerUp", pos, Quaternion.identity);

                        break;
                    }
                case 1:
                    {
                        currentNumOfPowerups++;
                        //powerUps.Add(Resources.Load<PowerUpCollectible>("SpeedPowerUp"));
                        PhotonNetwork.Instantiate("SpeedPowerUp", pos, Quaternion.identity);
                        break;
                    }
            }
        }
        public void removeFromList()
        {
            currentNumOfPowerups--;
            //powerUps.RemoveAt(powerUps.Count - 1);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {

            if (stream.IsWriting)
            {
                stream.SendNext(currentNumOfPowerups);
                stream.SendNext(maxNumOfPowerUps);
                stream.SendNext(timer);
            }
            else
            {
                currentNumOfPowerups = (int)stream.ReceiveNext();
                maxNumOfPowerUps = (int)stream.ReceiveNext();
                timer = (float)stream.ReceiveNext();
            }

        }
    }
}
