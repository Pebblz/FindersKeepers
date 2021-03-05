using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
namespace com.pebblz.finderskeepers
{
    public class PlayerNameText : MonoBehaviour
    {
        private TextMesh Tm;
        private GameObject Player;
        private GameObject camera;

        void Start()
        {
            camera = GameObject.FindGameObjectWithTag("MainCamera");
            Tm = GetComponent<TextMesh>();
            Tm.text = GetComponentInParent<PhotonView>().Owner.NickName;
        }
        void Update()
        {
            //it makes sure the that if the player is the parent to not find the parent
            //just so you don't constently see the name above your head but
            //but the other players see your name
            if (transform.parent.gameObject.tag != "Player")
            {
                if (Player == null)
                {
                    Player = GameObject.FindGameObjectWithTag("Player");
                }
            }
            if (Player != null)
            {
                Vector3 temp = camera.gameObject.transform.forward;
                temp.y = 90;
                transform.rotation = Quaternion.LookRotation(-temp);
            }
        }
    }
}
