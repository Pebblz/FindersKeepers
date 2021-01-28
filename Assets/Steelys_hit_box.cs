using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steelys_hit_box : MonoBehaviour
{
    Steely_AI steely;
    // Start is called before the first frame update
    void Start()
    {
        steely = GetComponentInParent<Steely_AI>();
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag != "Not Pick Upable")
    //    {
    //        steely.PickUp(other.gameObject);
    //    }
    //}
}
