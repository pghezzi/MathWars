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
    
    int startingHearts;
    int startingCoins; 
    int totalWaves;
    int currWave;
    int hearts;
    int coins;
    int MAX_COINS = 999;
    Level level;

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
    }

    // Update is called once per frame
    void Update()
    {
        heartsText.text = hearts.ToString();
        coinsText.text = coins.ToString();
        wavesText.text = $"WAVE {currWave}/{totalWaves}";
    }
    
    public void loseHearts(int numHeartsLost)
    {
        if (hearts > 0)
        {
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
            coins = Mathf.Clamp(coins - numCoinsLost, 0, MAX_COINS); 
        }
    }
    
    public void gainCoins(int numCoinGained)
    {
        if (coins < MAX_COINS)
        {
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
            currWave += 1;
        }
    }
    
    public void resetCoins()
    {
        coins = startingCoins;
    }
}