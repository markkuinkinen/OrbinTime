using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class textHandler : MonoBehaviour
{
    public LocationHandler locHandler;
    public NPCandQuestHandler questHandler;

    public PlayerController playerController;
    MonsterController MonController;

    public bool canTalk;        // checks if its possible to talk to npc
    public string talkingTo;    // if can talk, checks gameobject name of npc 
    public bool blueOrbMessage;
    public GameObject blueOrbCollider;
    public GameObject wellCollider;

    // for the main text box
    public GameObject textBox;
    public Image spriteToDisplay;
    public Sprite[] npcSprites; //0-4: chief, farmer, gravekeeper, pilot, starterguy, medsFrom, medsTo, penguin
    public Text showedText;
    public Text dots;
    public Text question;
    public Text yesArrow;
    public Text noArrow;
    bool inQuestion;
    public string whatToSay;
    bool gameFinished;

    public SpriteRenderer chiefsRend;
    public SpriteRenderer playersRend;
    public Sprite oldChief;
    public Sprite newChief;
    public bool updatedChief;

    public GameObject white;
    public float timer;

    public GameObject propellerGrid;
    public GameObject penguinObject;
    public bool propellerGiven;
    public bool inSnow;
    public bool canFlyToSnow;
    public bool canFlyBack;

    public int reader;

    string[] chiefDialogue = { "Greetings Hero! I'm so glad you could make it.", "The reason I hired you is to help me fix this portal. I had a little accident which caused an interdimensional energy surge.", "As a result of that I've lost my power source and the townspeople have reported a few ..strange incidents.", "If you can help retrieve the orbs that flew all over the place then you will be greatly rewarded!", "Oh, and I suppose you could help the townspeople while you're at it, they might even know where the orbs are.", "Come back to me if you can collect all 5 orbs." };
    string[] chiefDialogue1 = { "You might be able to find the orbs by helping the townspeople. Come back to me if you can find them all." };
    string[] postOrbChief = { "Oh how delightful, you actually managed to collect all of the orbs, as expected of a so-called hero.", "hehehe..", "AHAHAHA! You fool! Now I can finally take off this ridiculous outfit.",
                                "Ahh, much better. I have to say that outfit was utterly ridiculous, I can't believe the townspeople actually fell for it!", "Like seriously, my beard was made of whipped cream..",
                                "Either way thanks to your valiant efforts I can now link this portal to the demon realm to finally take full control of the island, and then the world!", "Oh? What's that? You actually want to try and stop me? You must be out of your mind!", "...",
                                "Well in that case pick up this staff and lets duke it out!" };

    string[] preFarmerDia = { "Top of the morning to ya! My crops are dying and we're all in immense danger, you should really go and talk to the Chief." };    // before main quest starts
    string[] farmerDialogue = { "HERO! HELP!", "The slimes.. THEY'RE EVERYWHERE!", "..." };
    string[] farmerDialogue1 = { "Oh thank goodness, the farm is now safe.. But for some reason the well isn't refilling, could you check on that for me?" };
    string[] farmerDialogue2 = { "Finally! They're all gone.", "Thank you, Hero. You've saved my crops, and this town. Here, take this.", "You have been given a Green Orb!" };
    string[] postOrbFarmer = { "Thank you hero, now the villagers won't starve to death." };

    string[] preGkDia = { "The graveyard is too dangerous to visit right now so I can't let you in, unless you're some sort of hero. In that case you should see the Chief." };        // before main quest starts
    string[] gkDialogue = { "Hi there, I'm guessing you're here to help. Praise the sun for we're all in GrAvE danger! (heh)", "But seriously my business is totally dying! (heheh)" ,"Usually I'm fine with dead people, but these ones are a little.. aggressive.", "If you could put those poor souls to rest I'd be eternally grateful.", "Also could you come back and tell me when it's safe to go out there.. " };
    string[] gkDialogue1 = { "Ooooh thank you so much for clearing out the graveyard! Did you know it's the dead centre of town?", "...", "People are dying to get there!", "Ha aha haa, but in all seriousness, the people who live at this town can't actually be buried there.", "...", "BECAUSE THEY'RE STILL ALIVE! AHAHAHAHAAA! I'm a hoot aren't I?", "But here, one of the zombies dropped this, I don't know what it is but take it as a thank you gift from me.", "You have been given a Pink Orb!" };
    string[] postGkDialogue = { "Thanks again, now I have loads of time to work on my comedy routine!", "Oh and there's something oddly suspicious about the south western side of the graveyard, but I can't quite put my finger on it.", "Perhaps I'm just imagining things.." };

    string[] prePilotDia = { "Man.. How am I going to figure this out... Maybe the Chief will know what to do." };     // before main quest starts
    string[] pilotDialogue1 = { "G'day, if you're looking to fly somewhere well unfortunately you're out of luck.",
                                "I've somehow managed to lose my propeller, If you can find it for me that'd be real dandy."};
    string[] pilotDialogue2 = { "Oh wow! You actually found my propeller. Why is it wet...?", "...", "What? You found it behind the waterfall?? Darn kids!", "Well at least it's cleaner now, thanks for bringing it back!",
                                "Come and talk to me again if you want to fly somewhere, I've got you covered."};
    string[] pilotQuestion1 = { "I didn't expect to see you back so soon. Would you like to go to the snow town?", "Well then let's get going!" };
    string[] chestDialogue = { "You have obtained the propeller!" };   // for the propeller quest

    string[] pilotSnow1 = { "She flies pretty well, huh?", "Come and talk to me again when you'd like to head back." };
    string[] pilotSnow2 = { "Everything wrapped up here? Then let's head out!" };

    string[] sgDialogue = { "Blimey! We didn't think you'd arrive so soon!", "Our town is in dire need of help, our Chief told me you'd arrive and that you should see him. You can find him on the north side of town." };
    bool sgSpokenTo = false;
    string[] sgDialogue1 = { "Go and see the Chief, he's in the house on the northern side of the town, up the stairs. You can't miss it." };
    string[] sgDialogue2 = { "I hope you can sort this out for us, I need to get back to doing absolutely nothing.", "But safely.. you know?" };

    string[] preMedsDia = { "Oh woe is me, if only there was a hero to help me, maybe the Chief knows of one." };        // before main quest starts
    string[] medsFromDialogue = { "Hey, you must be that 'Hero' or whatever that the Chief mentioned.", "My grandpa is suuuuper sick and really needs this medicine. I can't deliver it to him because I need to call my boyfriend or something.", "Oh and by the way he lives in the snow. My sister is there looking after him so you should give her this and she'll take care of the rest.", "You have been given the medicine." };
    string[] medsFromDialogue1 = { "Oh and by the way I was working at the flower stand and saw some kids causing trouble by the waterfall..", "Kids are weird." };
    string[] medsFromDialogue2 = { "Oh it's you again. My sister sent me a text saying you delivered the medicine in time.", "She also said Grandpa removed me from his will... So thanks for that!", "...", "At least my boyfriend is rich."};

    string[] medsToDialogue0 = { "Grandpa really needs medicine...", "My sister should have delivered it by now.. why isn't she here yet?!" };
    string[] medsToDialogue1 = { "Hello.. Oh you have medicine with you? ", "Did my sister ask you to deliver this?","...", "That lazy harpie!", "But thanks for delivering the medicine, grandpa will be healthy in no time thanks to you!", "Here, I don't know what this is, but you can take it.", "You have been given a Orange Orb." };
    string[] medsToDialogue2 = { "Thanks again for the medicine! Grandpa is already looking better." };

    string[] penguinDialogue = { "The penguin looks deeply into your eyes.", "You can sense that it needs your help.", "Help the lost penguin get home." };
    string[] blueOrbText = { "You feel something in your pocket, the penguin must have somehow placed it there.", "You have obtained the blue orb!" };

    string[] finalDialogue = { "Well I guess that's it then.. the town no longer needs saving, what a shocking twist that turned out to be.", "All that work and I didn't even get a reward...", "I suppose being a hero is rewarding enough.. Maybe it's time for me to settle down, I've had enough of this wayfaring hero life.", "Hmmm actually, maybe the old chief was on to something.. I have an idea!", "Ahahahaha", "AHAHAHAHAHAHAHA", "empty line"};

    bool inConversation;

    void Start()
    {
        gameFinished = false;
        reader = 0;
        MonController = FindObjectOfType<MonsterController>();
    }

    void Update()
    {   
        if (locHandler.postBossFight)
        {
            locHandler.inBossFight = false;
            showedText.text = finalDialogue[reader];
            textBox.SetActive(true);
            spriteToDisplay.enabled = false;
            if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return)) && locHandler.postBossFight && reader < finalDialogue.Length)
            {
                reader++;
                if (reader == 4)
                {
                    locHandler.boss_Skin.SetActive(false);
                    playerController.changeSprite();
                }
            }
            else if (reader == finalDialogue.Length - 1)
            {
                textBox.SetActive(false);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        talkToNPC();
        
        if (reader > 0)     
        {
            playerController.canMove = false;
        }
        else
        {
            playerController.canMove = true;
        }

        if (blueOrbMessage)
        {
            playerController.canMove = false;
            playerController.ChangeAnimationState("Idle_down");
            popUpTextHandler(blueOrbText);
        }
    }

    public bool ePressed()
    {
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return)) && canTalk)       // increases reader value by 1 if E pressed
        {
            if (talkingTo.ToString() == "farmerOutside")
            {
                if (playerController.facingLeft)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (talkingTo.ToString() == "gravekeeperOutside")
            {
                if (playerController.facingRight)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (playerController.facingUp)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void finalConversation()
    {
        textBox.SetActive(true);
        spriteToDisplay.enabled = false;
        showedText.text = finalDialogue[reader];
    }

    public void popUpTextHandler(string[] popUpDialogue)
    {
        textBox.SetActive(true);
        playerController.canMove = false;
        playerController.canTurn = false;
        spriteToDisplay.enabled = false;

        if (reader < popUpDialogue.Length)
        {
            if (locHandler.postBossFight && reader == 4)
            {
                locHandler.boss_Skin.SetActive(false);
                playerController.sRend.sprite = npcSprites[0];
            }

            showedText.text = popUpDialogue[reader];
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("E");
                reader++;
            }
        }
        else
        {
            if (blueOrbMessage)
            {
                questHandler.orbCollected("blue");
                playerController.canMove = true;
                playerController.canTurn = true;
                blueOrbCollider.SetActive(false);
            }
            Debug.Log("here???");
            textBox.SetActive(false);
            blueOrbMessage = false;
            reader = 0;
        }
    }

    void conversationHandler(string[] npcDialogue) 
    {
        spriteToDisplay.enabled = true;
        textBox.SetActive(true);

        if (reader < npcDialogue.Length)        // if reader isnt at the end
        {
            if (talkingTo.ToString() == "chief" && reader == 3 && questHandler.orbsCollected)
            {
                updatedChief = true;
                locHandler.boss_Skin.SetActive(true);
                spriteToDisplay.sprite = newChief;
                chiefsRend.sprite = newChief;
                
            }

            showedText.text = npcDialogue[reader];
            if (ePressed())
            {
                reader++;
            }
        }
        else if (ePressed())                                                                    // Section that closes text
        {
            if (talkingTo.ToString() == "chief")
            {
                if (questHandler.preBossFight)
                {
                    Debug.Log("in the bossfight now");
                    questHandler.orbsCollected = false;
                }
                if (questHandler.orbsCollected)
                {
                    questHandler.preBossFight = true;
                }
                questHandler.startMainQuest();
            }
            else if (talkingTo.ToString() == "starterguy")
            {
                if (!questHandler.mainQuestStartedBool)
                {
                    sgSpokenTo = true;
                }
            }
            else if (talkingTo.ToString() == "gravekeeper" && MonController.zombiesKilled && !questHandler.pinkOrb)
            {
                questHandler.orbCollected("pink");
            }
            else if (talkingTo.ToString() == "penguin")
            {
                locHandler.inPenguinGame = true;
                locHandler.spawn(-61.7f, -41.2f);
                playerController.ChangeAnimationState("playerIdle_down");
                playerController.facingDown = true;
                Debug.Log("Should be in penguin game");
                playerController.isAlive = true;
                playerController.canTurn = false;
                penguinObject.GetComponent<Transform>().position = new Vector2(23.94f, -64.09f);
            }
            else if (talkingTo.ToString() == "medsFrom")
            {
                if (questHandler.mainQuestStartedBool && !questHandler.medicine)
                {
                    questHandler.medicine = true;
                }
            }
            else if (talkingTo.ToString() == "medsTo" && !questHandler.medicineDeliveredBool)
            {
                if (questHandler.medicine && !questHandler.orangeOrb)
                {
                    questHandler.medicineDeliveredBool = true;
                    questHandler.orbCollected("orange");
                }
            }
            else if (talkingTo.ToString() == "farmer" && questHandler.slimeGameComplete && !questHandler.greenOrb)
            {
                questHandler.orbCollected("green");                     // green orb given
            }
            else if (talkingTo.ToString() == "pilot")
            {
                if (questHandler.propeller)
                {
                    if (!locHandler.inSnow && !canFlyToSnow)
                    {
                        canFlyToSnow = true;
                        propellerGiven = true;
                    }
                    else if (!locHandler.inSnow && canFlyToSnow)
                    {
                        locHandler.flyToSnow();
                        locHandler.inSnow = true;
                        //canFlyToSnow = false;
                        locHandler.inPilotHouse = false;
                        //textBox.SetActive(false);               //maybe this fixes
                    }
                    else if (locHandler.inSnow && !canFlyBack)
                    {
                        canFlyBack = true;
                    }
                    else if (locHandler.inSnow && canFlyBack)
                    {
                        locHandler.flyBack();
                        locHandler.inSnow = false;
                        canFlyBack = false;
                        canFlyToSnow = true;
                        //textBox.SetActive(false);               //maybe this fixes 
                    }
                }
            }

            if (!questHandler.orbsCollected)
            {
                textBox.SetActive(false);
                playerController.canMove = true;
                reader = 0;

            }

            if (questHandler.preBossFight == true)
            {
                textBox.SetActive(false);
                locHandler.inBossFight = true;
                locHandler.enterBossFight();
                Debug.Log("close text and enter boss fight");
            }
        }
    }

    void talkToNPC()
    {
        if (ePressed())
        {
            talk(talkingTo);
        }
    }

    void talk(string name)
    {
        if (name == "chief" && playerController.canTurn)
        {
            if (!updatedChief)
            {
                spriteToDisplay.sprite = npcSprites[0];
            }
            else
            {
                spriteToDisplay.sprite = newChief;
            }
            
            if (questHandler.orbsCollected)
            {
                conversationHandler(postOrbChief);
            }
            else if (questHandler.mainQuestStartedBool)
            {
                conversationHandler(chiefDialogue1);
            }
            else if (!questHandler.mainQuestStartedBool)
            {
                conversationHandler(chiefDialogue);
            }
        }
        else if (name == "farmerOutside")
        {
            spriteToDisplay.sprite = npcSprites[1];
            conversationHandler(preFarmerDia);
        }
        else if (name == "gravekeeperOutside")
        {
            spriteToDisplay.sprite = npcSprites[2];
            conversationHandler(preGkDia);
        }
        else if (name == "farmer")
        {
            spriteToDisplay.sprite = npcSprites[1];

            if (questHandler.greenOrb)
            {
                conversationHandler(postOrbFarmer);
            }
            else if (questHandler.slimeGameComplete)
            {
                conversationHandler(farmerDialogue2);
            }
            else if (MonController.slimesKilled)
            {
                conversationHandler(farmerDialogue1);
                wellCollider.SetActive(true);
            }
            else if (questHandler.mainQuestStartedBool)
            {
                conversationHandler(farmerDialogue);
            }
            else if (!questHandler.mainQuestStartedBool)
            {
                conversationHandler(preFarmerDia);
            }
        }
        else if (name == "gravekeeper")
        {
            spriteToDisplay.sprite = npcSprites[2];
            if (!questHandler.mainQuestStartedBool)
            {
                conversationHandler(preGkDia);
            }
            else if (!MonController.zombiesKilled && questHandler.mainQuestStartedBool)
            {
                conversationHandler(gkDialogue);
            }
            else if (MonController.zombiesKilled && !questHandler.pinkOrb)
            {
                conversationHandler(gkDialogue1);
            }
            else if (questHandler.pinkOrb)
            {
                conversationHandler(postGkDialogue);
            }
        }
        else if (name == "starterguy")
        {
            spriteToDisplay.sprite = npcSprites[4];
            
            if (questHandler.mainQuestStartedBool)
            {
                conversationHandler(sgDialogue2);
            }
            else if (sgSpokenTo)
            {
                conversationHandler(sgDialogue1);
            }
            else if (!questHandler.mainQuestStartedBool)
            {
                conversationHandler(sgDialogue);
            }
        }
        else if (name == "medsFrom")
        {
            spriteToDisplay.sprite = npcSprites[6];

            if (!questHandler.mainQuestStartedBool)
            {
                conversationHandler(preMedsDia);
            }
            else if (!questHandler.medicineDeliveredBool && !questHandler.medicine)
            {
                conversationHandler(medsFromDialogue);
            }
            else if (!questHandler.medicineDeliveredBool && questHandler.medicine)
            {
                conversationHandler(medsFromDialogue1);
            }
            else if (questHandler.medicineDeliveredBool)
            {
                conversationHandler(medsFromDialogue2);
            }
        }
        else if (name == "medsTo")
        {
            spriteToDisplay.sprite = npcSprites[5];
            if (!questHandler.medicine && !questHandler.medicineDeliveredBool)        // if medicine not delivered & doesnt have medicine
            {
                conversationHandler(medsToDialogue0);
            }
            else if (questHandler.medicine && !questHandler.medicineDeliveredBool)     // if medicine not delivered & HAS medicine
            {
                conversationHandler(medsToDialogue1);
            }
            else if (questHandler.medicineDeliveredBool)// && questHandler.medicineStatusChecker())
            {
                conversationHandler(medsToDialogue2);    //if has medicine and delivered
            }
        }
        else if (name == "penguin")
        {
            spriteToDisplay.sprite = npcSprites[7];
            conversationHandler(penguinDialogue);
        }
        else if (name == "pilot")
        {
            spriteToDisplay.sprite = npcSprites[3];
            if (!questHandler.mainQuestStartedBool)     // if the main quest hasn't started
            {
                conversationHandler(prePilotDia);
            }
            else if (locHandler.inSnow)                            // if in the snow
            {
                if (!canFlyBack)        
                {
                    conversationHandler(pilotSnow1);
                }
                else
                {
                    conversationHandler(pilotSnow2);
                }
            }
            else if (questHandler.mainQuestStartedBool && !questHandler.propeller)
            {
                conversationHandler(pilotDialogue1);    //asks before prop given
            }
            else if (questHandler.mainQuestStartedBool && questHandler.propeller && !propellerGiven)       
            {
                conversationHandler(pilotDialogue2);    //asks when giving prop
            }
            else if (questHandler.mainQuestStartedBool && questHandler.propeller && propellerGiven)
            {
                conversationHandler(pilotQuestion1);    //asks when in town
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "NPC")
        {
            canTalk = true;
            talkingTo = other.gameObject.name;
        }

        if (other.tag == "blueOrbText")
        {
            Debug.Log(playerController.canMove + " line 478");
            blueOrbMessage = true;
            playerController.canMove = false;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "NPC")
        {
            canTalk = false;
            textBox.SetActive(false);
            reader = 0;
        }
    }
}
