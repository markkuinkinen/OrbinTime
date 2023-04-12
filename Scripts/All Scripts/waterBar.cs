using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class waterBar : MonoBehaviour
{
    private slimeGameManager slimeManager;
    private Slider slider;

    void Start()
    {
        slimeManager = FindObjectOfType<slimeGameManager>();
        slider = GetComponent<Slider>();
        slider.value = 1;
        slider.maxValue = 30;
    }

    void Update()
    {
        slider.value = slimeManager.slimeWater;
    }
}
