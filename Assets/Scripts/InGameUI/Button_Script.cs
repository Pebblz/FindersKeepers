using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class Button_Script : MonoBehaviourPunCallbacks
{
    /*Flowery Box
     * Programmer: Patrick Naatz
     * Objective, put all the button functions into a single script
     */

    /// <summary>
    /// Quits the game
    /// </summary>
    public void Quit()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
            print("Works");
        }
        Application.Quit();
    }

    /// <summary>
    /// Loads main scene
    /// </summary>
    public void MainScene()
    {

            PhotonNetwork.Disconnect();
            print("Works2");
        
        SceneManager.LoadScene("Launcher");
    }

    /// <summary>
    /// Loads credit scene
    /// </summary>
    public void CreditScene()
    {
        SceneManager.LoadScene("Credits");
    }

    /// <summary>
    /// Loads the How To Play Scene
    /// </summary>
    public void HowToPlayScene()
    {
        SceneManager.LoadScene("HowToPlay");
    }

    /// <summary>
    /// Opens the feedback form in a new tab
    /// </summary>
    public void Feedback()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSetF1gEaPddTyZkeTUrGenpkXh-FmJ8iOQrpkEK4qoCXqiGMg/viewform?usp=sf_link");
    }

    /// <summary>
    /// Rejoins the lobby
    /// </summary>
    public void PlayAgain()
    {
        //this can be tricky because of room codes
        var networkManager = FindObjectOfType<NetworkManager>();
        networkManager.ResetToLobbyEvent();
        PhotonNetwork.LoadLevel("Lobby_" + PhotonNetwork.CurrentRoom.PlayerCount);   
    }
}
