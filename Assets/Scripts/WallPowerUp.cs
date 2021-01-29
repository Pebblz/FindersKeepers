using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPowerUp : PowerUp
{
    public void activate(Player player)
    {
        player.pm.rb.isKinematic = true;
        player.freeLookCam.GetComponent<Cinemachine.CinemachineCollider>().enabled = false;
    }

    public void deactivate(Player player)
    {
        player.pm.rb.isKinematic = false;
        player.freeLookCam.GetComponent<Cinemachine.CinemachineCollider>().enabled = true;
    }
}
