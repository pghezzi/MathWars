using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Explosion : MonoBehaviour 
{


    ParticleSystem explosionParticles;
    private AudioSource explosionSound;
    void Start() {
        explosionSound = gameObject.GetComponent<AudioSource>();
        AudioClip audio = (AudioClip) Resources.Load("Explode.mp3");
        gameObject.AddComponent<ParticleSystem>();
        explosionParticles = gameObject.GetComponent<ParticleSystem>();
        explosionSound.clip = audio;
        explosionSound.Play();
    }

    void Update() {

    }
}