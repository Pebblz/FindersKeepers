using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace com.pebblz.finderskeepers
{
    public class Places : MonoBehaviour
    {
        Player[] players;
        [SerializeField] Text text;
        // Start is called before the first frame update
        void Start()
        {
            players = FindObjectsOfType<Player>();
        }

        Player[] Order(Player[] scores)
        {
            for (int current = 0; current < scores.Length; current++)
            {
                for (int after = current + 1; after < scores.Length; after++)
                {
                    if (scores[after].score > scores[current].score)
                    {
                        //swap scores
                        Player temp = scores[current];
                        scores[current] = scores[after];
                        scores[after] = temp;
                    }
                }
            }

            return scores;
        }

        // Update is called once per frame
        void Update()
        {
            players = Order(players);
            text.text = "";
            int incrementer = 1;
            foreach (Player player in players)
            {
                if (player != null)
                {
                    string name = player.GetComponent<PhotonView>().Owner.NickName;
                    string score = player.score.ToString();
                    //+incrementer.ToString()
                    text.text = text.text + "\n"  + " " + name + ": " + score;
                }
                incrementer++;
            }
        }
    }
}