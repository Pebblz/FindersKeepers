﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPowerUpCollectible : MonoBehaviour
{
    public PowerUp powerUp;
    public AudioClip pickUpNoise;

    public void Awake()
    {
        powerUp = new WallPowerUp();
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            if(collision.collider.gameObject.GetComponent<Player>().getPowerUp() != null)
            {
                collision.collider.gameObject.GetComponent<Player>().deactivatePowerUp();
            }
            collision.collider.gameObject.GetComponent<Player>().setPowerUp(powerUp);
            collision.collider.gameObject.GetComponent<Player>().activatePowerUp();
            Destroy(this.gameObject);
        }
    }
}
