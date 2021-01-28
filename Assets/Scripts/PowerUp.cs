using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PowerUp
{
    void activate(Player player);
    void deactivate(Player player);
}
