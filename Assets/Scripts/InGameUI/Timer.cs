﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace com.pebblz.finderskeepers
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] Text text;
        [SerializeField] int seconds = 30;


        [SerializeField] GameManager gameManager;
        // Start is called before the first frame update
        void Start()
        {
            seconds = gameManager.setTime();

            if (text == null)
            {//if text wasn't set
                text = gameObject.GetComponent<Text>(); // try to find the text on this object
                if (text == null)
                {//if text is still null
                    Destroy(this);
                }
            }

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
            gameManager.Continue();
            seconds = gameManager.setTime();
            StartCoroutine("CountDown");
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
}
