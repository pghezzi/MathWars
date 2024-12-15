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
        audioManager.PlaySFX(audioManager.click);
        SceneManager.LoadScene(level_name);
    }
    
    public void ToLevelMenu()
    {
        audioManager.PlaySFX(audioManager.click);
        SceneManager.LoadScene("LevelMenu"); 
    }
}

