using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_PauseMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject par_PM;
    [SerializeField] private GameObject par_PMContent;
    [SerializeField] private Button btn_ReturnToGame;
    [SerializeField] private Button btn_ReturnToMM;
    [SerializeField] private Button btn_Quit;

    [Header("Assignables")]
    [SerializeField] private GameObject thePlayer;

    //public but hidden variables
    [HideInInspector] public bool isPaused;

    //scripts
    private Player_Movement PlayerMovementScript;

    private void Awake()
    {
        PlayerMovementScript = thePlayer.GetComponent<Player_Movement>();
    }

    private void Start()
    {
        btn_ReturnToGame.onClick.AddListener(UnpauseGame);
        btn_ReturnToMM.onClick.AddListener(
            delegate { GetComponent<UI_Confirmation>().UIConfirmationRequest("pauseMenu",
                                                                             "returnToMM",
                                                                             "Do you want to go back to main menu?"); });
        btn_Quit.onClick.AddListener(
            delegate { GetComponent<UI_Confirmation>().UIConfirmationRequest("pauseMenu",
                                                                             "quit",
                                                                             "Do you want to go quit the game?"); });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
        }
        if (!isPaused
            && par_PM.activeInHierarchy)
        {
            UnpauseGame();
        }
        else if (isPaused
                 && !par_PM.activeInHierarchy)
        {
            PauseWithUI();
        }
    }

    //unpause and continue game
    public void UnpauseGame()
    {
        par_PMContent.SetActive(false);
        par_PM.SetActive(false);

        PlayerMovementScript.LoadPlayer();

        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isPaused = false;
    }

    //only pauses the game
    public void PauseWithoutUI()
    {
        PlayerMovementScript.UnloadPlayer();

        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        isPaused = true;
    }
    //pauses the game and opens the pause menu UI
    public void PauseWithUI()
    {
        PauseWithoutUI();

        par_PM.SetActive(true);
        par_PMContent.SetActive(true);
    }

    //switch to main menu scene
    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene(0);
    }

    //quit the game
    public void Quit()
    {
        Application.Quit();
    }
}