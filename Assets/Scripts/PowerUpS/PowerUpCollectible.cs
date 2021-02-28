﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpCollectible : MonoBehaviour
{
    public PowerUp powerUp;
    public AudioClip pickUpNoise;
    public PowerUpCollectible pc;

    public abstract void OnCollisionEnter(Collision collision);
}