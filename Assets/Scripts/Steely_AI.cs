using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.pebblz.finderskeepers
{
    public class Steely_AI : MonoBehaviour
    {

        /* Flower Box
         * Programmer: Pat Naatz
         * Bare minimal AI that picks things up and swaps them with other objects
         * 
         * ToDo
         * Finish the random drop item function
         * improve the AI direction choices
         * Make AI increase speed when it picks up an item
         * */

        Rigidbody rb;
        int speed = 1;

        //Current Direction
        int H = 0;
        int V = 0;

        //Pocket Variables
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
                //change directions
                H = Random.Range(-1, 2);
                V = Random.Range(-1, 2);

                yield return new WaitForSeconds(Random.Range(1, 10)); //wait for some time between 1 and 10 seconds
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
        {      //dont move walls and stuff         dont move the same thing twice
            if (other.tag == "Moveable" && other.gameObject != redList)
            {
                PickUp(other.gameObject);
            }
        }

        void PickUp(GameObject gameObject)
        {

            if (holdingSomething != null)
            { //if holding something
                PutObjectHere(gameObject.transform.position);
                DirectionChange();
            }

            //pick up the gameobject
            gameObject.transform.parent = transform;
            gameObject.transform.position = transform.position + new Vector3(0, 1, 0); //todo make position dynamic
            holdingSomething = gameObject;
        }

        void PutObjectHere(Vector3 posistion)
        {
            //put down item
            holdingSomething.transform.position = posistion;
            holdingSomething.transform.parent = null;

            //set up so the AI doesn't instantly pick it back up
            redList = holdingSomething;
        }
    }
}