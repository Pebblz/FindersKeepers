using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Photon.Pun;
using System.Linq;

public class WinOrLoseScript : MonoBehaviourPunCallbacks
{
    /*Flower Box
     * Programmer: Patrick Naatz
     * 
     * Show the win or lose screen depending on whether they won or lost
     * 
     * Added:
     *  The Audio Struct
     *  Auto changing podiums to match your place
     *  removed the camera problem
     *  Added temp sound
     *  reimplemented the win and lose functions
     *  updated the animation handling
     *  edited the load in time to be more dramatic
     *  endless regions
     * Todo:
     *  Change quickRot so the rotation is set and doesnt need to exist
     *  Finish Play Again Button
     */

    #region variables
    #region pre set
    [SerializeField] Text text;

    //Audio Variables
    [System.Serializable] struct Audio_Variables
    {
        public AudioSource audioSource;
        public AudioClip winSound, loseSound;
    }
    [SerializeField] Audio_Variables audioVaraibles;
    
    [SerializeField] Transform[] podiums;

    [SerializeField] GameObject quickRot;
    #endregion
    #region run time variables
    Player[] players;

    List<Player> winners = new List<Player>();
    List<Player> losers = new List<Player>();
    #endregion
    #endregion

    bool SinglePlayer
    {
        get
        {
            return (players.Length == 1);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Get all players
        players = FindObjectsOfType<Player>();

        DisableUselessPodiums();

        //order the scores
        Order();
        CheckForTies();

        //win or lose stuff
        PlayAnimations();    
        MakeLastPlayerCry();

        //the reveal
        Display();
        StartCoroutine("Reveal");
    }

    #region visual changes

    #region podiums

    private void ReplacePodium(int podiumNumber)
    {
        if (podiumNumber != 0)
        {//to prevent an error
            Transform oldPodium = podiums[podiumNumber];
            //save position
            Vector3 position = oldPodium.position;
            
            Destroy(oldPodium.gameObject);

            //generate replacement podium
            Transform replacementPodium = podiums[podiumNumber - 1];
            Transform newPodium = Instantiate(replacementPodium.gameObject).transform;

            //set new podium location logic according to ISROT rules
            newPodium.localScale = replacementPodium.localScale;
            newPodium.rotation = replacementPodium.rotation;
            newPodium.position = position;

            //replace old podium with new one
            podiums[podiumNumber] = newPodium;
        }
    }

    #region disabling podiums
    void DisableUselessPodiums()
    {
        //disable all the podiums that wont be used
        for(int i = players.Length; i < podiums.Length; i++)
        {
            podiums[i].gameObject.SetActive(false);
        }
    }

    void DisableLastPlacePodium()
    {
        if (!SinglePlayer)
        {
            if (!winners.Contains(players[players.Length - 1]))
            {//if everyone didnt tie for first
                //Generate the list of podiums
                List<int> podiumsToDestroy = GenerateBigestLosersList();

                //disable the last place podiums
                foreach (int i in podiumsToDestroy)
                {
                    podiums[i + winners.Count].gameObject.SetActive(false); //disables the podiums
                }  
            }
        }
    }
    #endregion 
    #endregion

    #region animations
    //note the win animation is handled in the win function

    private void PlayAnimations()
    {
        //set relevant animations
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
    }

    void MakeLastPlayerCry()
    {
        if (!SinglePlayer && winners.Count != players.Count())
        {
            List<int> biggestLosers = GenerateBigestLosersList();
            if (biggestLosers.Count != players.Length)
            {//if everyone doesnt win
                foreach (int i in biggestLosers)
                {
                    players[i + winners.Count].GetComponent<Animator>().SetBool("Fourth", true); //make player cry
                }
            }
        }
    }
    #endregion

    #region locational logic
    void Display()
    {
        //moves players away from eachother to prevent accidental bumping
        PrepPlayersForPlacement();

        PlacePlayersAccordingly();
    }


    private void PlacePlayersAccordingly()
    {
        //we have to use RPC to move each character because each game loads in character in a different order but they all have the photon transform on them.
        if (PhotonNetwork.IsMasterClient)
        {//since we have to use an RPC we only want everything to happen once, so we only allow the master client to go in
            int incrementation = 0;
            foreach (Player player in players)
            {
                //move to space above designated podium
                GetComponent<PhotonView>().RPC("MoveHere", RpcTarget.All, incrementation, player.name);
                
                //prep for next incrementation
                incrementation++;
            }
        }
    }

    /// <summary>
    /// Prevents teleportation problems occuring during winscene
    /// </summary>
    /// <param name="podiumNumber"></param>
    /// <param name="playerName"></param>
    [PunRPC]
    public void MoveHere(int podiumNumber, string playerName)
    {
        Transform player = GameObject.Find(playerName).transform; //find player by name

        //move them to position, reverse ISROT
        player.transform.rotation = quickRot.transform.rotation;
        player.transform.position = podiums[podiumNumber].position + Vector3.up * 30; //creates a 3 second fall for dramatic effect
    }

    private int PrepPlayersForPlacement()
    {
        int incrementation = 1;
        foreach (Player player in players)
        {
            //prevents movement
            Rigidbody rb = player.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

            //moves to isolated location
            player.transform.position = new Vector3(incrementation * 100, incrementation * 100, incrementation * 100);

            //setup for next incrementation
            incrementation++;
        }

        return incrementation;
    }

    IEnumerator Reveal()
    {
        //properly organize players to drop at same or different times
        List<List<Player>> order = new List<List<Player>>(); //have to use nested list because people can tie with eachother
        foreach(Player player in players)
        {
            order.Add(new List<Player>());
        }
        order[0] = winners;
        for(int i = 1, p = winners.Count; p < players.Length; p++)
        {
            if(order[i].Count == 0)
            {//if this order is empty add the player
                order[i].Add(players[p]);
            } else if(IsTied(order[i][0],players[p]))
            {//if this player is tied with this order add the palyer
                order[i].Add(players[p]);
            } else
            {//if neither progress to next order and back a player so they dont get forgotten
                i++;
                p--;
            }
        }

        foreach (List<Player> ties in order)
        {
            foreach (Player player in ties)
            {
                player.GetComponent<Rigidbody>().useGravity = true; //turns on gravity for said player
            }

            yield return new WaitForSeconds(1); //1 second wait

            foreach (Player player in ties)
            {
                if (IsLocalPlayer(player))
                {
                    PlayWinOrLose(player);
                }
            }
        }

        //when all places are displayed
        DisableLastPlacePodium();
    }
    #endregion

    #endregion

    #region organizing

    #region Tie Logic

    #region IsTied
    bool IsTied(int index1, int index2)
    {
        return IsTied(players[index1], players[index2]); //overload
    }

    bool IsTied(Player player1, Player player2)
    {
        return player1.score == player2.score;
    }
    #endregion

    private void CheckForTies()
    {
        winners.Add(players[0]); //add the known winner to the list

        for(int i = 0; i < players.Length; i++)
        {
            if(i != 0)
            {//if not first player (blocks error)
                if(IsTied(players[i], players[i - 1]))
                {//if player is tied with the one before it
                    ReplacePodium(i);

                    if (IsTied(players[i], winners[0]))
                    {//if player is tied with the winner
                        winners.Add(players[i]);
                    }
                }
            }
        }

        PopulateLosers();
    }
    #endregion

    private void PopulateLosers()
    {
        foreach(Player player in players)
        {
            if (!winners.Contains(player))
            {//if not a winner
                losers.Add(player);
            }
        }
    }

    private List<int> GenerateBigestLosersList()
    {
        if(losers.Count == 0)
        {
            return new List<int>(new int[] { 0, 1, 2, 3 });
        }

        int index = losers.Count - 1; //starts on player in last

        //Generate list of last place players
        List<int> podiumsToDestroy = new List<int>();
        podiumsToDestroy.Add(index); //add known biggest loser to list

        while (index - 1 != -1 && IsTied(losers[index], losers[index - 1]))
        {//while in bounds and has the same score as last place person
            //add to the destroy list
            podiumsToDestroy.Add(index - 1);

            //setup for next loop
            index--;
        }

        return podiumsToDestroy;
    }

    void Order()
    {
        players = players.OrderByDescending(p => p.score).ToArray();
    }
    #endregion

    #region Win or Lose
    void PlayWinOrLose(Player player)
    {
        if (winners.Contains(player))
        {//if local player won
            Win(player);
            return;
        }
        
        //otherwise if you didnt win, oyu lost
        Lose();
    }

    void Win(Player winner)
    {
        PlaySound(audioVaraibles.winSound);
        text.text = "YOU WON!";
        winner.GetComponent<Animator>().SetBool("First", true); //plays the winning animation now because the 1-4 animations are already playing
    }

    void Lose()
    {
        PlaySound(audioVaraibles.loseSound);
        text.text = "You Lost";
    }
    #endregion

    #region helper functions
    Player localPlayerCache = null;
    bool IsLocalPlayer(Player player)
    {
        if (localPlayerCache == null)
        {//if not cached
            bool result = player.GetComponent<PhotonView>().Controller.IsLocal;
            if (result)
            {
                localPlayerCache = player;
                return true;
            }
        } else
        {
            if(player == localPlayerCache)
            {
                return true;
            }
        }
        
        return false;
    }

    void PlaySound(AudioClip audioClip)
    {
        audioVaraibles.audioSource.PlayOneShot(audioClip);
    }
    #endregion
}
