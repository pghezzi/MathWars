using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    public void LoadLevel(string level_name)
    {
        SceneManager.LoadScene(level_name);
    }
    
    public void ToLevelMenu()
    {
        SceneManager.LoadScene("LevelMenu"); 
    }
}

