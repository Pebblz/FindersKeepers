using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] Text text;
    //[SerializeField] int minutes = 1;
    [SerializeField] int seconds = 90;
    // Start is called before the first frame update
    void Start()
    {
        if(text == null)
        {//if text wasn't set
            text = gameObject.GetComponent<Text>(); // try to find the text on this object
            if(text == null)
            {//if text is still null
                Destroy(this);
            }
        }
        //seconds += minutes * 60;
        //minutes = 0;

        seconds++; //buffer for coroutine

        //start timer
        StartCoroutine("CountDown");
    }

    //the actual timer
    IEnumerator CountDown()
    {
        while (seconds != 0)
        {//while not done
            //count down
            seconds--;

            UpdateText(seconds);

            //wait for 1 second
            yield return new WaitForSeconds(1); //WaitForSeconds is effect by timescale
        }

        //when timer ends
        text.text = "Times up";
    }

    void UpdateText(int seconds)
    {
        text.text = Parse(seconds);
    }

    string Parse(int seconds)
    {
        //set minutes
        int minutes = seconds / 60;
        seconds %= 60; //remove minutes from seconds

                                            //if less than 10 seconds add a 0 infront
        return minutes.ToString() + ":" + ((seconds < 10) ? "0" : "") + seconds.ToString();
    }
}
