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
        {
            text = gameObject.GetComponent<Text>();
        }
        //seconds += minutes * 60;
        //minutes = 0;
        StartCoroutine("CountDown");
    }

    IEnumerator CountDown()
    {
        while (seconds != 0)
        {
            seconds--;
            text.text = Parse(seconds);

            yield return new WaitForSeconds(1);
        }

        text.text = "Times up";
    }

    string Parse(int seconds)
    {
        int minutes = seconds / 60;
        seconds %= 60;

        return minutes.ToString() + ":" + ((seconds < 10) ? "0" : "") + seconds.ToString();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
