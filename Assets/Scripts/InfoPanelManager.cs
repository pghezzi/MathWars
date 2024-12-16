using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using System.Net.NetworkInformation;

public class InfoPanelManager : MonoBehaviour
{
    public TMP_Text wavesText; 
    public TMP_Text coinsText;
    public TMP_Text heartsText;
    public GameObject TowerPlacer;
    public GameObject winScreen;
    public GameObject loseScreen;
    
    int startingHearts;
    int startingCoins; 
    int totalWaves;
    public int currWave;
    int hearts;
    int coins;
    int MAX_COINS = 999;
    public bool isGameWon = false;
    public bool isGameLost = false;
    Level level;

     
    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        level = GameObject.Find("Level").GetComponent<Level>();
        if (level == null)
        {
            throw new Exception("InfoPanelManager: Could not find Level component in scene");
        }
        LevelData levelData = level.loadLevelData(level.level_name);
        
        startingHearts = levelData.startingHearts;
        hearts = startingHearts;
        
        startingCoins = levelData.startingCoins;
        coins = startingCoins;
        
        totalWaves = levelData.numWaves;
        currWave = 1;
        
        heartsText.text = startingHearts.ToString(); 
        coinsText.text = startingCoins.ToString();
        wavesText.text = $"WAVE {currWave}/{totalWaves}";
        Instantiate(TowerPlacer);
    }

    // Update is called once per frame
    void Update()
    {
        heartsText.text = hearts.ToString();
        coinsText.text = coins.ToString();
        wavesText.text = $"WAVE {currWave}/{totalWaves}";
        checkIfLostLevel();
        // checkIfWonLevel();
    }
    
    public void loseHearts(int numHeartsLost)
    {
        if (hearts > 0)
        {
            audioManager.PlaySFX(audioManager.loseHeart);
            hearts = Mathf.Clamp(hearts - numHeartsLost, 0, startingHearts);
        }
    }
    
    public void gainHearts(int numHeartsGained)
    {
        if (hearts < startingHearts)
        {
            hearts = Mathf.Clamp(hearts + numHeartsGained, 0, startingHearts);
        }
    }
    
    public void loseCoins(int numCoinsLost)
    {
        if (coins > 0)
        {
            audioManager.PlaySFX(audioManager.spentMoney);
            coins = Mathf.Clamp(coins - numCoinsLost, 0, MAX_COINS); 
        }
    }
    
    public void gainCoins(int numCoinGained)
    {
        if (coins < MAX_COINS)
        {
            audioManager.PlaySFX(audioManager.earnedMoney);
            coins = Mathf.Clamp(coins + numCoinGained, 0, MAX_COINS);
        }
    }
    
    public bool canAfford(int cost)
    {
        return coins - cost >= 0 ? true : false;
    }
    
    public void resetHealth()
    {
        hearts = startingHearts;
    }
    
    public void incrementWave()
    {
        if (currWave < totalWaves)
        {
            audioManager.PlaySFX(audioManager.newWave);
            currWave += 1;
        }
    }
    
    public void checkIfLostLevel()
    {
        if (!isGameLost && hearts <= 0)
        {
            isGameLost = true; 
            audioManager.PlaySFX(audioManager.lost); 
            Instantiate(loseScreen);
        }
    }
    
    // public void checkIfWonLevel()
    // {
    //     // we will get enemiesLeft from WaveManager
    //     // if (!isGameWon && enemiesLeft <= 0 ) -- update when we total num enemies
    //     if (!isGameWon && hearts > 0 )
    //     {
    //         Instantiate(winScreen);
    //     }
    // }
    
    
    public void resetCoins()
    {
        coins = startingCoins;
    }
}
