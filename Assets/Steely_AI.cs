using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steely_AI : MonoBehaviour
{

    Rigidbody rb;
    int speed = 1;

    int H = 0;
    int V = 0;

    GameObject holdingSomething = null;

    GameObject redList = null;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine("DirectionChange");
    }

    // Update is called once per frame
    void Update()
    {
        #region Movement

        Vector3 tempVect = new Vector3(H, 0, V);
        tempVect = tempVect.normalized * speed * Time.deltaTime;
        rb.MovePosition(transform.position + tempVect);
        #endregion
    }

    IEnumerator DirectionChange()
    {
        while (true)
        {
            H = Random.Range(-1, 2);
            V = Random.Range(-1, 2);

            yield return new WaitForSeconds(Random.Range(1, 10));
        }
    }
    //IEnumerator DropObject()
    //{Debug.Log("Dropped");
    //    while(holdingSomething != null)
    //    {
    //          yield return new WaitForSeconds(Random.Range(10, 60));
    //        PutObjectHere(transform.position + new Vector3(holdingSomething.GetComponent<Renderer>().bounds.size.x * H * -1, 0, holdingSomething.GetComponent<Renderer>().bounds.size.x * V * -1));
    //        holdingSomething = null;
          
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("triggered");
        if(other.tag == "Moveable" && other.gameObject != redList)
        {
            //Debug.Log("internal log");
            PickUp(other.gameObject);
        }
    }

    void PickUp(GameObject gameObject)
    {
        
        if (holdingSomething != null)
        {
            PutObjectHere(gameObject.transform.position);
            DirectionChange();
        }
        gameObject.transform.parent = transform;
        gameObject.transform.position = transform.position + new Vector3(0, 1, 0);
        holdingSomething = gameObject;
    }

    void PutObjectHere(Vector3 posistion)
    {
        
        holdingSomething.transform.position = posistion;
        holdingSomething.transform.parent = null;
        redList = holdingSomething;
    }
}
