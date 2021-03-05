using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
namespace com.pebblz.finderskeepers
{
    public class PlayerNameText : MonoBehaviourPunCallbacks
    {
        private TextMesh Tm;
        private GameObject Player;
        private GameObject camera;

        void Start()
        {
            Tm = GetComponent<TextMesh>();
            Tm.text = "";
        }
        void Update()
        {
            if (!photonView.IsMine)
            {
                if (camera == null)
                {
                    camera = GameObject.Find("Main Camera");
                }
                if (Player == null)
                {
                    Tm.text = GetComponentInParent<PhotonView>().Owner.NickName;
                    Player = GameObject.FindGameObjectWithTag("Player");
                }
                if (Player != null && camera != null)
                {
                    Vector3 temp = camera.gameObject.transform.forward;
                    //temp.y = 90;
                    transform.rotation = Quaternion.LookRotation(-temp);
                    //transform.LookAt(Player.transform);
                    //float rotYofX = Mathf.Lerp(transform.rotation.y, -transform.rotation.y, transform.position.x);
                    //float rotYofY = Mathf.Lerp(transform.rotation.y, -transform.rotation.y, transform.position.y);
                    //float rotY = (rotYofX + rotYofY) / 2;
                    //transform.rotation = new Quaternion(0,rotY,0,transform.rotation.w);


                }
            }
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name == "WinOrLose")
            {
                Tm.text = "";
            }
        }
    }
}
