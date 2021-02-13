using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Places : MonoBehaviour
{
    Player[] players;
    [SerializeField] Text text;
    // Start is called before the first frame update
    void Start()
    {
        players = FindObjectsOfType<Player>();
    }

   

    void Order()
    {
        players = players.OrderByDescending(p => p.score).ToArray();

    }

    // Update is called once per frame
    void Update()
    {
        Order();
        text.text = "";
        int incrementer = 1;
        foreach (Player player in players)
        {
            if (player != null)
            {
                text.text = text.text + "\n" + incrementer.ToString() + " " + player.gameObject.name;
            }
            incrementer++;
        }
    }
}
