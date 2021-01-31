using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TodoList : MonoBehaviour
{
    /*Flower Boxing again
     * Programmer Patrick Naatz
     * this class when initiated randomly splits up the pickupable objects amungst whatever amount of players there are
     * It DOES allow player to have the same object as eachother on their list and this was done intentionally
     */

    //[SerializeField] Text text;
    [SerializeField]Image image;
    [SerializeField]Image image2;
    [SerializeField]Image image3;

    [SerializeField] GameManager gameManager;
    Dictionary<PickUpAbles, bool> list = new Dictionary<PickUpAbles, bool>();

    void Start()
    {
        //if(text == null)
        //{ //if text isnt filled
        //    text = GetComponent<Text>(); //fill the text
        //    if(text == null)
        //    {//if text is still not filled
        //        Destroy(text);
        //    }
        //}

        FillList();
        PrintList();
    }

    void FillList()
    {
        //player count
        int playerCount = Object.FindObjectsOfType<Player>().Length;

        //get list of pickupables in game
        List<PickUpAbles> pickUpAbleList = new List<PickUpAbles>(Object.FindObjectsOfType<PickUpAbles>());

        
        int objectsPerPerson = pickUpAbleList.Count / playerCount;

        //fill list randomly
        int place = 0;
        while (list.Count < objectsPerPerson)
        {//while still filling list

            //recalculate place
            place += Random.Range(0, playerCount);
            place %= pickUpAbleList.Count - 1;

            //add to list and remove from available pickupable's
            list.Add(pickUpAbleList[place], false);
            pickUpAbleList.RemoveAt(place);
        }
    }

    public void PrintList()
    {
        ////if (gameManager.listActive())
        ////{//if we want the list to show
        //    text.text = "List: \n";
        //    foreach (KeyValuePair<PickUpAbles, bool> kvp in list)
        //    {//foreach item in list
        //        text.text += '\n' + kvp.Key.gameObject.name; //add name to list
        //    }
        ////} else
        //{//if we dont
        //  //  text.text = "";
        //}

        Debug.Log("Here");

        FillImage(image);
        Debug.Log("2");
        FillImage(image2);
        Debug.Log("3");
        FillImage(image3);
        //foreach(Image image in images)
        //{
        //    FillImage(image);
        //}
    }

    void FillImage(Image img)
    {
        List<PickUpAbles> left = new List<PickUpAbles>();
        foreach (PickUpAbles obj in list.Keys)
        {
            if (!list[obj])
            {
                left.Add(obj);
            }
        }
        int nextObject = Random.Range(0, left.Count);
        PickUpAbles newObject = left.ToArray()[nextObject];
        Debug.Log(newObject.gameObject.name);
        img.sprite = newObject.image;

        //added to coordinate with pick up spawner
        left[nextObject].tag = "PointsPickUp"; //sets tag
        left[nextObject].IsThisOBJForPoints = true; //set true for points
        FindObjectOfType<PickUpableSpawner>().FindOBJ();
    }

    //Call this function when a pickupable is picked up
    //public void PickUpObject(PickUpAbles pickUpAble)
    //{
    //    if (list.ContainsKey(pickUpAble))
    //    {
    //        //declares as picked up
    //        list[pickUpAble] = true;
    //        Debug.Log(list[pickUpAble]);

    //        //TODO change the list to signify said item was picked up
    //        list.Remove(pickUpAble);
    //        PrintList();
    //    }
    //}

    public void ObjectFound(PickUpAbles obj)
    {
        if (list.ContainsKey(obj))
        {
            
            if(image.sprite == obj.image)
                {
                    FillImage(image);
                }
            else if(image2.sprite == obj.image)
            {
                FillImage(image2);
            } else if(image3.sprite == obj.image)
            {
                FillImage(image3);
            }
            list[obj] = true;
        }
    }
}
