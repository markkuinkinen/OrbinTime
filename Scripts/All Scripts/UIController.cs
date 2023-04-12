using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject gameControls;

    public void loadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void loadControls()
    {
        gameControls.SetActive(true);
    }

    public void closeControls()
    {
        gameControls.SetActive(false);
    }
}
