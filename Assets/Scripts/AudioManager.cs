using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource SFXSource;
    
    [Header("UI Audio Clips")]
    public AudioClip background;
    public AudioClip click;
    public AudioClip won;
    public AudioClip lost;
    public AudioClip spentMoney;
    public AudioClip earnedMoney;
    public AudioClip loseHeart;
    public AudioClip correctAnswer;
    public AudioClip wrongAnswer;
    public AudioClip newWave; 
    public AudioClip timerTick;

    [Header("Tower Audio Clips")]
    public AudioClip builtTower; 
    public AudioClip towerShooting;
    
    [Header("Enemy Audio Clips")]
    public AudioClip enemySpawn;
    public AudioClip enemyHit;
    
    float HIGH_VOLUME = 0.1f;
    float LOW_VOLUME = 0.05f; 
    
    public static AudioManager instance;
    
    void Awake()
    {
        // if an additional audioManager is created destroy it as we only want one
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
    
    public void setVolumeLow(AudioSource src)
    {
        src.volume = LOW_VOLUME;
    }
    
    public void setVolumeHigh(AudioSource src)
    {
        src.volume = HIGH_VOLUME;
    }
}
