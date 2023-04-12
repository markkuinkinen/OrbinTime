using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireScript : MonoBehaviour     //Added chain blocking path to this since its disabled at the same time
{
    Animator anim;
    public NPCandQuestHandler questHandler;
    public GameObject chain;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (questHandler.blueOrb)
        {
            anim.SetBool("fireOff", true);
            chain.SetActive(false);
        }
    }
}
