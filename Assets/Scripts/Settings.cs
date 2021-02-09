using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    /*Box
     * Programmer: Patrick Naatz
     * Objective: Make a script for setting values for game play then communicating them into the game
     */

    [SerializeField] GameMode gameMode;
    bool canAdjustTime = true;

    // Start is called before the first frame update
    void Start()
    {
        //Load last saved settings
    }

    #region Time Management

    #region UI triggered functions
    /// <summary>
    /// Change the value of the slider to match the input field
    /// Recommed setting this in the after finished function
    /// </summary>
    public void TimeRelevance(Slider slider)
    {
        if (canAdjustTime)//Added because the two related objects will recursicelly call eternally
        {
            //get values
            InputField from = EventSystem.current.currentSelectedGameObject.GetComponent<InputField>();

            uint minutes = 0, seconds = 0; //unsigned because there is no negative time

            if (from.text.Contains(":"))
            {//if there is distinguished difference between minutes and seconds 

                const int minute = 0, second = 1; //for array use
                
                //split the values
                string[] values = from.text.Split(':');
                if (values.Length == 1)
                {//if either the seconds or minutes is missing
                    if (from.text.IndexOf(":") == 0)
                    {
                        seconds = uint.Parse(values[0]);
                    }
                    else
                    {
                        minutes = uint.Parse(values[minute]);
                    }
                }
                else
                {
                    seconds = uint.Parse(values[second]);
                    minutes = uint.Parse(values[minute]);
                }
            }
            else if (from.text.Length != 0)
            {
                seconds = uint.Parse(from.text);
            }

            //compress number for proper formatting
            seconds += minutes * 60;
            minutes = seconds / 60;
            seconds %= 60;

            //set values
            canAdjustTime = false;
            slider.value = minutes + (float)seconds / 100;

            //we reset the value of this slider because the user may not put in proper formatting
            canAdjustTime = false;
            SetTime(from, seconds, minutes);

        } else
        {//if recursion was prevented
            canAdjustTime = true;
        }
    }

    /// <summary>
    /// Set Time Relevance from Slider to inputfield
    /// </summary>
    /// <param name="inputField"></param>
    public void TimeRelevance(InputField inputField)
    {
        if (canAdjustTime) //added because the related items can recursively call eternally
        {
            //get values
            Slider from = EventSystem.current.currentSelectedGameObject.GetComponent<Slider>();
            float value = from.value;
            uint minutes = (uint)value;
            value -= minutes;
            uint seconds = (uint)(value * 60);

            canAdjustTime = false;

            SetTime(inputField, seconds, minutes);
        } else
        {//if recursion was prevented
            canAdjustTime = true;
        }
    }
    #endregion

    void SetTime(InputField inputField, uint seconds, uint minutes = 0)
    {
        //incase improper formatting
        minutes += seconds / 60;
        seconds %= 60;

        //make time string
        string time = (minutes != 0) ? minutes + ":" : ""; //add minutes
        time += ((seconds < 10) ? "0" : "") + seconds.ToString(); //add seconds

        inputField.text = time;
    }
    #endregion
}
