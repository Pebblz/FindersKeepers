using UnityEngine;
using System.Linq;
public class FindAllPickupables : MonoBehaviour
{
    public PickUpAbles[] AllGameObjects = new PickUpAbles[120];
    void Awake()
    {
        AllGameObjects =  FindObjectsOfType<PickUpAbles>();
    }
}
