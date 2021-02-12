using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class TelePorter : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject OtherTelePorter;

    public float TimeToTelePort;
    GameObject Player;
    public GameObject Q;
    GameObject camera;
    private void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
    }
    void Update()
    {

        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
        if (Vector3.Distance(this.gameObject.transform.position, Player.transform.position) < 2)
        {
            Q.SetActive(true);
            Vector3 temp = camera.gameObject.transform.forward;
            temp.y = 90;
            Q.transform.rotation = Quaternion.LookRotation(-temp);
        }
        else
        {
            Q.SetActive(false);
        }

        TimeToTelePort -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider col)
    {

        if (col.tag == "Player")
        {

            if (TimeToTelePort < 0 && Input.GetKey(KeyCode.Q))
            {
                col.transform.position = OtherTelePorter.transform.position;
                col.GetComponent<PlayerPickUp>().ResetPickUpPos();
                OtherTelePorter.GetComponent<TelePorter>().TimeToTelePort = .5f;

            }
        }

    }
}
