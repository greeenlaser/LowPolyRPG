using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //public but hidden variables
    [HideInInspector] public int sceneIndex;

    //scripts
    private UI_PauseMenu PauseMenuScript;

    private void Awake()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void Start()
    {
        if (sceneIndex == 1)
        {
            PauseMenuScript = GetComponent<UI_PauseMenu>();
            PauseMenuScript.UnpauseGame();
        }
    }
}