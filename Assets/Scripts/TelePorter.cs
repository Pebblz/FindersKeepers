using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelePorter : MonoBehaviour
{
    [SerializeField]
    GameObject OtherTelePorter;

    public float TimeToTelePort;
    // Update is called once per frame
    void Update()
    {
        TimeToTelePort -= Time.deltaTime;
    }
    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            if (TimeToTelePort <= 0)
            {
                col.transform.position = OtherTelePorter.transform.position;
                OtherTelePorter.GetComponent<TelePorter>().TimeToTelePort = 1;
            }
        }
    }
}
