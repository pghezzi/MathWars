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
    public TMP_Text waveTimer;
    public GameObject waveTimerPopUp;
    public GameObject TowerPlacer;
    public GameObject winScreen;
    public GameObject loseScreen;

    private float time;
    
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
    WaveManager waveManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        waveManager = GameObject.Find("WaveManager").GetComponent<WaveManager>();
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

        time = waveManager.timeBetweenWaves;

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
        checkIfWonLevel();
        WaveTimer();
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
    
    public void checkIfWonLevel()
    {
        if (!isGameWon && hearts > 0 && waveManager.totalEnemies <= 0)
        {
            isGameWon = true;
            audioManager.PlaySFX(audioManager.won);
            Instantiate(winScreen);
        }
    }
    
    public void WaveTimer()
    {
        
        if (waveManager.betweenWaves)
        {
            Debug.Log("In between Waves");
            waveTimerPopUp.SetActive(true);
            waveTimer.enabled = true;
            if (currWave < totalWaves)
            {
                time -= Time.deltaTime;
                waveTimer.text = $"Wave {currWave} Complete!\nNext wave starting in {Math.Ceiling(time)}";
            }
            else
            {
                waveTimer.text = $"Wave {currWave} Complete!\nDefeat All Enemies";
            }
        }
        
        else
        {
            waveTimerPopUp.SetActive(false);
            waveTimer.enabled = false;
            time = waveManager.timeBetweenWaves;
        }
    }
    
    public void resetCoins()
    {
        coins = startingCoins;
    }
}
