using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    Light thisLight;
    [SerializeField]
    float minWaitTimeForFlicker;
    [SerializeField]
    float maxWaitTimeForFlicker;
    // Start is called before the first frame update
    void Start()
    {
        thisLight = GetComponent<Light>();
        StartCoroutine(Flashing());
    }

    IEnumerator Flashing()
    {
        while (true)
        {
            //it'll wait a random time between min and max times set
            yield return new WaitForSeconds(Random.Range(minWaitTimeForFlicker, maxWaitTimeForFlicker));
            //it'll flicker on and off 
            thisLight.enabled = ! thisLight.enabled;
        }
    }
}
