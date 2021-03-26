using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondScript : MonoBehaviour
{
    void Update()
    {
        transform.rotation = new Quaternion(90, 0, 0, 0);
        transform.position = new Vector3(transform.position.x, 9, transform.transform.position.z);
    }
}
