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

    [SerializeField] Text text;
    Dictionary<PickUpAbles, bool> list = new Dictionary<PickUpAbles, bool>();

    void Start()
    {
        if(text == null)
        { //if text isnt filled
            text = GetComponent<Text>(); //fill the text
            if(text == null)
            {//if text is still not filled
                Destroy(text);
            }
        }

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
            place %= pickUpAbleList.Count;

            //add to list and remove from available pickupable's
            list.Add(pickUpAbleList[place], false);
            pickUpAbleList.RemoveAt(place);
        }
    }

    void PrintList()
    {
        text.text = "List: \n";
        foreach(KeyValuePair<PickUpAbles, bool> kvp in list)
        {//foreach item in list
            text.text += '\n' + kvp.Key.gameObject.name; //add name to list
        }
    }

    //Call this function when a pickupable is picked up
    public void PickUpObject(PickUpAbles pickUpAble)
    {
        if (list.ContainsKey(pickUpAble))
        {
            //declares as picked up
            list[pickUpAble] = true;
            Debug.Log(list[pickUpAble]);

            //TODO change the list to signify said item was picked up
            list.Remove(pickUpAble);
            PrintList();
        }
    }
}
