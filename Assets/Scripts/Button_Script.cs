using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_Script : MonoBehaviour
{
    /*
     * Added Settings button
     */
      
    /// <summary>
    /// Loads the settings scene
    /// </summary>
    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }
}
