using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinOrLoseScript : MonoBehaviour
{
    /*Flower Box
     * Programmer: Patrick Naatz
     * 
     * Show the win or lose screen depending on whether they won or lost
     */

    [SerializeField] Text text;

    //sound variables
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip loseSound;

    [SerializeField] Camera[] cameras;
    [SerializeField] Transform[] podiums;
    //[SerializeField] RawImage[] images;

    Player[] players;

    [SerializeField]GameObject quickRot;
    // Start is called before the first frame update
    void Start()
    {
        //Get Scores from server
        players = FindObjectsOfType<Player>();
 
        //order the scores
        players = Order(players);

        for(int j = 0; j < players.Length; j++)
        {
            Player player = players[j];
            switch(j)
            {
                case 0:
                    player.GetComponent<Animator>().SetBool("First", true);
                    break;
                case 1:
                    player.GetComponent<Animator>().SetBool("Second", true);
                    break;
                case 2:
                    player.GetComponent<Animator>().SetBool("Third", true);
                    break;
                case 3:
                    player.GetComponent<Animator>().SetBool("Fourth", true);
                    break;
            }
        }

        if(players.Length != 4)
        {
            players[players.Length - 1].GetComponent<Animator>().SetBool("Fourth", true);
        }

        //foreach(RawImage image in images)
        //{
        //    image.gameObject.SetActive(false);
        //}
        for(int i = players.Length; i < podiums.Length; i++)
        {
            podiums[i].gameObject.SetActive(false);
        }
        Display(players);
        StartCoroutine("Flash");
    }





    void Display(Player[] players)
    {
        int incrementation = 0;
        foreach(Player player in players)
        {
            player.gameObject.transform.position = podiums[incrementation].position + new Vector3(0,50 + incrementation * 50,0);
            player.gameObject.transform.rotation = quickRot.transform.rotation;
            Camera cam = cameras[incrementation];
            cam.transform.position = player.transform.position - new Vector3(0, 0, 4);
            cam.transform.parent = player.transform;
            incrementation++;
            //player.enabled = false;
            //player.freeLookCam.SetActive(false);
            //player.GetComponent<Player_Movement>().enabled = false;
        }
    }

    IEnumerator Flash()
    {
        //int incrementation = 0;
        //foreach (Player player in players)
        //{
            yield return new WaitForSeconds(6);
        //    //images[incrementation].gameObject.SetActive(true);
        //    incrementation++;
        //}

        podiums[players.Length-1].gameObject.SetActive(false);
    }

    Player[] Order(Player[] scores)
    {
        for(int current = 0; current < scores.Length; current++)
        {
            for(int after = current + 1; after < scores.Length; after++)
            {
                if(scores[after].score > scores[current].score)
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
    
    void Win()
    {
        audioSource.PlayOneShot(winSound);
        text.text = "YOU WON!";
    }

    void Lose()
    {
        audioSource.PlayOneShot(loseSound);
        text.text = "You Lost";
    }

    #region Button Functions
    public void PlayAgain()
    {
        //Get roomcode from before
        //SceneManager.LoadScene("Lobby 1");  commented out because I need to ask josh if scene loading works this way
    }

    public void Quit()
    {
       Application.Quit();
    }
    #endregion
}
