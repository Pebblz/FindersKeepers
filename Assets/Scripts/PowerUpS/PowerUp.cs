using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.pebblz.finderskeepers
{
    public interface PowerUp
    {
        void activate(Player player);
        void deactivate(Player player);
    }
}
