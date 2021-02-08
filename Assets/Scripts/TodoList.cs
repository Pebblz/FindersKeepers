using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TodoList : MonoBehaviourPunCallbacks, IPunObservable
{
    /*Flower Boxing again
     * Programmer Patrick Naatz
     * this class when initiated randomly splits up the pickupable objects amungst whatever amount of players there are
     * It DOES allow player to have the same object as eachother on their list and this was done intentionally
     * 
     * Completed:
     * Revamp the images so it works out of an array
     * Timer changes available in the game manager script
     * Made it so the same image doesnt display at the same time
     */

    [SerializeField] Image[] images;

    [SerializeField] GameManager gameManager;
    Dictionary<PickUpAbles, bool> list = new Dictionary<PickUpAbles, bool>();

    private void Start()
    {
        //disable all images
        foreach(Image image in images)
        {
            image.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// call this function when you want to load and display the list
    /// </summary>
    public void Active()
    {
        //activate all images
        foreach(Image image in images)
        {
            image.gameObject.SetActive(true);
        }


        FillList();
        LoadList();
    }

    /// <summary>
    /// Fills the todo list with apropriate objects
    /// </summary>
    public void FillList()
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
            place %= pickUpAbleList.Count;

            //add to list and remove from available pickupable's
            list.Add(pickUpAbleList[place], false);
            pickUpAbleList.RemoveAt(place);
        }
    }

    /// <summary>
    /// Fills all the images in the UI
    /// </summary>
    public void LoadList()
    {
        foreach(Image image in images)
        {
            FillImage(image);
        }
    }


    void FillImage(Image img)
    {

        //generate list of objects that still haven't been found
        List<PickUpAbles> left = new List<PickUpAbles>();
        foreach (PickUpAbles obj in list.Keys)
        {
            if (!list[obj] && IsAlreadyDisplayed(obj))
            { //if object not already found by player
                left.Add(obj);
            }
        }

        //chooses a random object from the list
        int nextObject = Random.Range(0, left.Count);
        if (left[nextObject] != null)
        { //there should be no objects found that dont exist
            PickUpAbles newObject = left.ToArray()[nextObject];

            img.sprite = newObject.image;


            //added to coordinate with pick up spawner
            left[nextObject].tag = "PointsPickUp"; //sets tag
            left[nextObject].IsThisOBJForPoints = true; //set true for points
        } else
        {
            Debug.Log("tried to load object that doesnt exist");
        }

        FindObjectOfType<PickUpableSpawner>().FindOBJ(); //called to coordinate something with pickupablespawner
    }

    bool IsAlreadyDisplayed(PickUpAbles pickUpAble)
    {
        foreach(Image image in images)
        {
            if(image.sprite == pickUpAble.image)
            {
                return true;
            }
        }
        return false;
    }

    //DIDNT DELETE THIS because it could be used in the future
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
    //        LoadList();
    //    }
    //}

    /// <summary>
    /// Call this function when you try to collect a object on the list, it will refill the image
    /// </summary>
    /// <param name="obj"></param>
    public void ObjectFound(PickUpAbles obj)
    {
        if (list.ContainsKey(obj))
        {//if object was on list
            foreach (Image image in images) {
                if (image.sprite == obj.image)
                {//if image is the target image
                    list[obj] = true; //set found to true
                    FillImage(image); //refill image
                    break; //no need to loop anymore
                }
            }

        }
    }

    //this function exists to comply with IPunObservable, I dont know why, ask Jimmy
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
        }
        else
        {
        }
    }
}
