﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
                    temp.y = 90;
                    transform.rotation = Quaternion.LookRotation(-temp);
                }
            }
        }
    }
}