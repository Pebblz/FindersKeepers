using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taser : MonoBehaviour
{
    //this is so the player doesn't shoot himself 
    public GameObject PlayerWhoShotThis;

    float DestroyTimer = .7f;
    void Update()
    {
        DestroyTimer -= Time.deltaTime;
        if (DestroyTimer <= 0)
            Destroy(this.gameObject);
    }
    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player" && col.gameObject != PlayerWhoShotThis)
        {
            col.GetComponent<Player>().StunPlayer();
            Destroy(this.gameObject);
        }
        
    }
}
