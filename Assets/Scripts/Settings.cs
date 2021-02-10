using System;
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

    public static GameMode gameMode;
    bool canAdjustTime = true;

    [SerializeField] Dropdown gameModeDropDownMenu;
    Dictionary<string, Transform> editedPanels = new Dictionary<string, Transform>();

    //Prefabs
    public GameObject panelPrefab;
    public GameObject togglePrefab;
    public GameObject timePrefab;

    // Start is called before the first frame update
    void Start()
    {
        LoadGameMode();
        //Load last saved settings
    }

    public void LoadGameMode()
    {
        if(gameMode != null)
        {
            editedPanels[gameMode.name].gameObject.SetActive(false);
        }
        gameMode = Resources.Load<GameMode>("Game Modes/" + gameModeDropDownMenu.options[gameModeDropDownMenu.value].text);
        if (editedPanels.ContainsKey(gameMode.name))
        {
            editedPanels[gameMode.name].gameObject.SetActive(true);
        } else
        {
            LoadPanel();
        }
    }

    private void LoadPanel()
    {
        Transform panel = Instantiate(panelPrefab, transform).transform;
        editedPanels.Add(gameMode.name, panel);
        Transform togglePanel = panel.transform.Find("Toggles");
        foreach(GameMode.Toggles toggle in gameMode.toggles)
        {
            Toggle UIToggle = Instantiate(togglePrefab, togglePanel).GetComponent<Toggle>();
            UIToggle.name = toggle.name;
            UIToggle.transform.localScale += Vector3.one * this.GetComponent<CanvasScaler>().scaleFactor;
            UIToggle.onValueChanged.AddListener(delegate
            {
                Toggle(UIToggle.name);
            });
            UIToggle.transform.Find("Label").GetComponent<Text>().text = toggle.name;
        }

        Transform timingPanel = panel.transform.Find("Timers");
        foreach (GameMode.TimeInSeconds timer in gameMode.Times)
        {
            Transform UITimer = Instantiate(timePrefab, timingPanel).transform;

            UITimer.name = timer.name;
            UITimer.Find("Text").GetComponent<Text>().text = timer.name;

            UITimer.transform.localScale += Vector3.one * this.GetComponent<CanvasScaler>().scaleFactor;

            Slider slider = UITimer.Find("Slider").GetComponent<Slider>();

            UITimer.Find("Min").GetComponent<Text>().text = timer.min.ToString();
            slider.minValue = timer.min;

            UITimer.Find("Max").GetComponent<Text>().text = timer.max.ToString();
            slider.maxValue = timer.max;

            slider.value = timer.current;
            slider.onValueChanged.AddListener(delegate
            {
                Timer(UITimer.name);
            });
        }
    }

    public void Toggle(string name)
    {
        GameMode.Toggles thisToggle;
        foreach(GameMode.Toggles toggle in gameMode.toggles)
        {
            if(toggle.name == name)
            {
                thisToggle = toggle;
            }
        }
        thisToggle.value = editedPanels[gameMode.name].Find("Toggles").Find(name).GetComponent<Toggle>().isOn;
    }

    public void Timer(string name)
    {
        //get values
        Slider from = EventSystem.current.currentSelectedGameObject.GetComponent<Slider>();
        GameMode.TimeInSeconds thisTimer;
        foreach (GameMode.TimeInSeconds timer in gameMode.Times)
        {
            if (timer.name == name)
            {
                thisTimer = timer;
            }
        }
        float value = from.value;
        int minutes = (int)value;
        value -= minutes;
        int seconds = (int)(value * 100.0f);
        thisTimer.current = minutes * 60 + seconds;
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
