using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : PowerUp
{
    public void activate(Player player)
    {
        float speed = 8;
        player.pm.setSpeed(speed);
    }

    public void deactivate(Player player)
    {
        float speed = 5;
        player.pm.setSpeed(speed);
    }
}
