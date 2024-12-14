using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pauseScreen;
    
    public void handlePause()
    {
        Debug.Log("PAUSE"); 
        Instantiate(pauseScreen);
    }

    public void handleResume()
    {
        Debug.Log("RESUME");
        Destroy(pauseScreen);
    }
    
    public void handleQuitLevel()
    {
        SceneManager.LoadScene("LevelMenu");
    }

}
