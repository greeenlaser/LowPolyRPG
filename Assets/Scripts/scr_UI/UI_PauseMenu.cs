using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class UI_PauseMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject par_PM;
    [SerializeField] private GameObject par_PMContent;
    [SerializeField] private Button btn_ReturnToGame;
    [SerializeField] private Button btn_ShowSettings;
    [SerializeField] private Button btn_ReturnToPM;
    [SerializeField] private Button btn_ReturnToMM;
    [SerializeField] private Button btn_Quit;

    [Header("Assignables")]
    [SerializeField] private GameObject thePlayer;

    //public but hidden variables
    [HideInInspector] public bool isPaused;
    [HideInInspector] public bool isConsoleOpen;

    //scripts
    private Manager_UIReuse UIReuseScript;
    private Player_Movement PlayerMovementScript;

    private void Awake()
    {
        UIReuseScript= GetComponent<Manager_UIReuse>();
        PlayerMovementScript = thePlayer.GetComponent<Player_Movement>();

        btn_ReturnToGame.onClick.AddListener(UnpauseGame);
        btn_ShowSettings.onClick.AddListener(ShowSettings);
        btn_ReturnToPM.onClick.AddListener(ReturnToPauseMenu);
        btn_ReturnToMM.onClick.AddListener(delegate { GetComponent<UI_Confirmation>().
            UIConfirmationRequest("pauseMenu",
                                  "returnToMM",
                                  "Do you want to go to the main menu?"); });
        btn_Quit.onClick.AddListener(delegate { GetComponent<UI_Confirmation>().
            UIConfirmationRequest("pauseMenu",
                                  "quit",
                                  "Do you want to go quit the game?"); });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
        }

        if (isPaused

            //checking if other UI programs
            //have not already paused the game
            && !isConsoleOpen

            && !par_PM.activeInHierarchy)
        {
            PauseWithUI();
        }
        else if (!isPaused
                 && par_PM.activeInHierarchy)
        {
            UnpauseGame();
        }
    }

    public void UnpauseGame()
    {
        isPaused = false;

        //first close pause menu if it is open
        if (par_PM.activeInHierarchy)
        {
            par_PM.SetActive(false);
            par_PMContent.SetActive(false);
            UIReuseScript.par_SettingsParent.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        //then unpause game if no other pause-dependant UI is open
        if (!isConsoleOpen)
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        PlayerMovementScript.LoadPlayer();
    }
    public void PauseWithoutUI()
    {
        PlayerMovementScript.UnloadPlayer();

        isPaused = true;

        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void PauseWithUI()
    {
        PauseWithoutUI();

        par_PM.SetActive(true);
        par_PMContent.SetActive(true);
        UIReuseScript.par_SettingsParent.SetActive(false);
    }

    public void ShowSettings()
    {
        par_PMContent.SetActive(false);
        UIReuseScript.par_SettingsParent.SetActive(true);
        UIReuseScript.SwitchToSettings();
        UIReuseScript.SwitchChoice(1);
    }
    public void ReturnToPauseMenu()
    {
        par_PMContent.SetActive(true);
        UIReuseScript.par_SettingsParent.SetActive(false);
    }
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif

#if UNITY_STANDALONE
        Application.Quit();
#endif
    }
}