using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCandQuestHandler : MonoBehaviour
{
    // Player variables
    public PlayerController player;
    public GameObject playerObject;
    private AudioSource playerAudio;

    // Quest variables
    public bool mainQuestStartedBool;
    public bool propeller;
    public bool preBossFight;
    public bool orbsCollected;
    public bool greenOrb;
    public bool blackOrb;
    public bool blueOrb;
    public bool pinkOrb;
    public bool orangeOrb;
    public bool medicine;
    public bool medicineDeliveredBool;
    public bool slimeGameComplete;
    
    public GameObject[] orbs;
    public GameObject[] orbShadows;

    public GameObject waterfallDoor;
    public BoxCollider2D slimeGameCollider;
    public GameObject pacmanGame;
    public GameObject preFarmer;
    public GameObject preGravekeeper;

    // Sound variables
    public AudioClip obtainedSound;
    public AudioClip completedSound;
    public AudioClip doorSound;
    public bool soundPlayed;

    // Crypt puzzle
    public static bool cryptPuzzle;

    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
    }

    public void playObtainedSound()
    {
        playerAudio.PlayOneShot(obtainedSound, 0.6f);
    }

    public void playCompletedSound()
    {
        playerAudio.PlayOneShot(completedSound, 0.5f);
    }

    public void playDoorSound()
    {
        playerAudio.PlayOneShot(doorSound, 0.5f);
    }

    void Update()
    {
        if (mainQuestStartedBool)
        {
            startMainQuest();
            waterfallDoor.SetActive(true);
            orbCompletionChecker();
        }
    }

    public void slimeGameCompleted()
    {
        slimeGameComplete = true;
        slimeGameCollider.enabled = false;
    }

    void orbCompletionChecker()
    {
        if (greenOrb && blackOrb && pinkOrb && blueOrb && orangeOrb)
        {
            orbsCollected = true;
        }
    }

    public void startMainQuest()
    {
        mainQuestStartedBool = true;
        preFarmer.SetActive(false);
        preGravekeeper.SetActive(false);
        for (int i = 0; i < orbShadows.Length; i++)
        {
            orbShadows[i].SetActive(true);
        }
    }

    public void orbCollected(string colour)
    {
        if (colour == "green") 
        {
            greenOrb = true;
            playerAudio.PlayOneShot(obtainedSound, 0.6f);
            orbs[0].SetActive(true);
        }
        else if (colour == "black")
        {
            blackOrb = true;
            if (!soundPlayed)
            {
                playerAudio.PlayOneShot(obtainedSound, 0.6f);
                soundPlayed = true;
            }
            orbs[1].SetActive(true);
        }
        else if (colour == "blue")
        {
            blueOrb = true;
            playerAudio.PlayOneShot(obtainedSound, 0.6f);
            orbs[2].SetActive(true);
        }
        else if (colour == "pink")  //
        {
            pinkOrb = true;
            playerAudio.PlayOneShot(obtainedSound, 0.6f);
            orbs[3].SetActive(true);
        }
        else if (colour == "orange")
        {
            orangeOrb = true;
            playerAudio.PlayOneShot(obtainedSound, 0.6f);
            orbs[4].SetActive(true);
        }
    }

    public void disableOrbs()
    {
        for (int i = 0; i <= 4; i++)
        {
            orbs[i].SetActive(false);
        } 
    }
}
