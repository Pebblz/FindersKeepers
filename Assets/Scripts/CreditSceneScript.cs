using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditSceneScript : MonoBehaviour
{
    #region Button Function
    public void Back()
    {
        SceneManager.LoadScene("Launcher");
    }
    #endregion
}
