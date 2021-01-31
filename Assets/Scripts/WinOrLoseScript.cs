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

    // Start is called before the first frame update
    void Start()
    {
        //Get Scores from server
        int thisPlayersScore = 0;
        int[] scores = { };

        //order the scores
        scores = Order(scores);


        if(thisPlayersScore == scores[0])
        {
            Win();
        } else
        {
            Lose();
        }
    }

    int[] Order(int[] scores)
    {
        for(int current = 0; current < scores.Length; current++)
        {
            for(int after = current + 1; after < scores.Length; after++)
            {
                if(scores[after] > scores[current])
                {
                    //swap scores
                    int temp = scores[current];
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
