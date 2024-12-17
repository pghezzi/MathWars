using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    
    public void LoadLevel(string level_name)
    {
        Time.timeScale = 1;
        audioManager.PlaySFX(audioManager.click);
        SceneManager.LoadScene(level_name);
    }
    
    public void ToLevelMenu()
    {
        Time.timeScale = 1;
        audioManager.PlaySFX(audioManager.click);
        SceneManager.LoadScene("LevelMenu"); 
    }
}

