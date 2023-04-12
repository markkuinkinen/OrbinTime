using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PenguinProgress : MonoBehaviour
{
    private Slider slider;
    private PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        slider = GetComponent<Slider>();
        
        slider.value = -42;
        slider.minValue = -255;
    }

    void Update()
    {
        slider.value = player.transform.position.y;
    }
}
