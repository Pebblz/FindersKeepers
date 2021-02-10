using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditSceneScript : MonoBehaviour
{
    /*Flower Box
     * Programmer: Patrick Naatz
     * Objective: Make a script that returns you from the credit scene to the main scene
     */

        /// <summary>
        /// Loads the Main scene (Launcher)
        /// </summary>
    #region Button Function
    public void Back()
    {
        //attach this function to the back button

        SceneManager.LoadScene("Launcher"); //loads main scene
    }
    #endregion
}
