using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPowerUp : PowerUp
{
    public void activate(Player player)
    {
        player.GetComponent<Rigidbody>().isKinematic = true;
        player.freeLookCam.GetComponent<Cinemachine.CinemachineCollider>().enabled = false;
    }

    public void deactivate(Player player)
    {
        player.GetComponent<Rigidbody>().isKinematic = false;
        player.freeLookCam.GetComponent<Cinemachine.CinemachineCollider>().enabled = true;
    }
}
