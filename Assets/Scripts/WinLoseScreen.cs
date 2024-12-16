using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseScreen : MonoBehaviour
{
    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    
    string level_name;
    
    void Start()
    {
        Time.timeScale = 0;
        level_name = GameObject.FindGameObjectWithTag("Level").GetComponent<Level>().level_name.Split('.')[0];
        // Capitalize first letter
        level_name = Char.ToUpper(level_name[0]) + level_name.Substring(1);
        Debug.Log($"level_name: {level_name}");
    }
    
    public void RestartLevel()
    {
        audioManager.PlaySFX(audioManager.click);
        Time.timeScale = 1;
        SceneManager.LoadScene(level_name);
    }
}
