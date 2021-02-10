using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Mode", menuName = "Game Mode", order = 1)]
public class GameMode : ScriptableObject
{
    public string name;

    //boolean settings
    [System.Serializable] public struct Toggles
    {
        public Toggles(string name, bool value)
        {
            this.name = name;
            this.value = value;
        }

        public string name;
        public bool value;
    }
    public Toggles[] toggles = { new Toggles("Randomized Rooms", true)};

    //time stuff
    [System.Serializable] public struct TimeInSeconds
    {
        public string name;
        public int min,max,current;
    }
    public TimeInSeconds[] Times;

    public void Continue()
    {

    }
}
