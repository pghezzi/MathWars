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
        // This line will pause anything that relies on time in Unity
        // If we want to still do something when timeScale is 0 we can use unscaledTime or realtime
        // Audio has a pause funciton if we need to pause it
        // This will still register user inputs so if we find a weird bug where after being paused
        // many actions are being performed we may want to add a isPaused flag across our scripts
        Time.timeScale = 0;
        Instantiate(pauseScreen);
    }

    public void handleResume()
    {
        Debug.Log("RESUME");
        Time.timeScale = 1;
        Destroy(pauseScreen);
    }
    
    public void handleQuitLevel()
    {
        SceneManager.LoadScene("LevelMenu");
        Time.timeScale = 1;
    }

}
