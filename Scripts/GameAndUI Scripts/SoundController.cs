using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public LocationHandler locHandler;
    public AudioSource audioPlayer;

    //background music
    public AudioClip mainMusic;
    public AudioClip slimeGameMusic;
    public AudioClip snowMusic;
    public AudioClip penguinGameMusic;
    public AudioClip bossFightMusic;
    public AudioClip creditsMusic;
    public AudioClip pacmanMusic;
    public AudioClip postBossFightMusic;
    public AudioClip graveyardMusic;

    public AudioClip zombieDeathSound;
    public AudioClip slimeDeathSound;

    public bool tunePlayed;

    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        locHandler = FindObjectOfType<LocationHandler>();
    }

    public void playZombieDeathSound()
    {
        audioPlayer.PlayOneShot(zombieDeathSound, 1.5f);
    }

    public void playSlimeDeathSound()
    {
        audioPlayer.PlayOneShot(slimeDeathSound, 1.1f);
    }

    void Update()
    {
        if (locHandler.inSlimeGame)
        {
            playMusic(slimeGameMusic);
        }
        else if (locHandler.inSnow)
        {
            if (locHandler.inPenguinGame)
            {
                playMusic(penguinGameMusic);
            }
            else
            {
                playMusic(snowMusic);
            }
        }
        else if (locHandler.inPacmanGame)
        {
            playMusic(pacmanMusic);
        }
        else if (locHandler.inBossFight)
        {
            playMusic(bossFightMusic);
        }
        else if (locHandler.postBossFight)
        {
            if (!tunePlayed)
            {
                audioPlayer.clip = postBossFightMusic;
                audioPlayer.loop = false;
                audioPlayer.Play();
                tunePlayed = true;
            }
        }
        else if (locHandler.inGraveyard)
        {
            playMusic(graveyardMusic);
        }
        else if (!locHandler.postBossFight && !locHandler.inGraveyard)
        {
            playMusic(mainMusic);
        }
        else
        {
            audioPlayer.Stop();
        }
    }

    void playMusic(AudioClip bgm)
    {
        if (!audioPlayer.isPlaying || audioPlayer.clip != bgm)
        {
            audioPlayer.clip = bgm;
            audioPlayer.Play();
        }
        else if (bgm == postBossFightMusic)
        {
            audioPlayer.loop = false;
            if (!audioPlayer.isPlaying)
            {
                audioPlayer.Stop();
            }
        }
    }
}
