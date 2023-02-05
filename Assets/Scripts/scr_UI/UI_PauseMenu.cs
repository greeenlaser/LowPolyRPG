using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UI_PauseMenu : MonoBehaviour
{
    [Header("Main pause menu UI")]
    [SerializeField] private GameObject par_PM;

    [Header("Pause menu content")]
    [SerializeField] private GameObject par_PMContent;
    [SerializeField] private Button btn_ReturnToGame;
    [SerializeField] private Button btn_ShowSettings;
    [SerializeField] private Button btn_ReturnToMM;
    [SerializeField] private Button btn_Quit;

    [Header("Settings menu content")]
    [SerializeField] private GameObject par_SettingsContent;
    [SerializeField] private GameObject par_GameSettingsContent;
    [SerializeField] private GameObject par_KeyBindingsContent;
    [SerializeField] private Button btn_ShowGameSettings;
    [SerializeField] private Button btn_ShowKeyBindings;
    [SerializeField] private Button btn_ReturnToPauseMenu;
    [SerializeField] private Button btn_Button1;
    [SerializeField] private Button btn_Button2;
    [SerializeField] private Button btn_Button3;

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
        btn_ReturnToPauseMenu.onClick.AddListener(ReturnToPauseMenu);

        btn_ShowSettings.onClick.AddListener(ShowSettings);
        btn_ShowGameSettings.onClick.AddListener(ShowGameSettings);
        btn_ShowKeyBindings.onClick.AddListener(ShowKeyBindings);

        btn_ReturnToMM.onClick.AddListener(delegate { GetComponent<UI_Confirmation>().
            UIConfirmationRequest("pauseMenu",
                                  "returnToMM",
                                  "Do you want to go back to main menu?"); });
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
        ReturnToPauseMenu();
    }

    //display the main settings UI
    public void ShowSettings()
    {
        par_SettingsContent.SetActive(true);
        ShowGameSettings();
    }
    //display all game settings
    public void ShowGameSettings()
    {
        btn_ShowGameSettings.interactable = false;
        btn_ShowKeyBindings.interactable = true;

        btn_Button1.onClick.RemoveAllListeners();
        btn_Button2.onClick.RemoveAllListeners();
        btn_Button3.onClick.RemoveAllListeners();
        btn_Button1.GetComponentInChildren<TMP_Text>().text = "General";
        btn_Button2.GetComponentInChildren<TMP_Text>().text = "Environment";
        btn_Button3.GetComponentInChildren<TMP_Text>().text = "Audio";

        par_KeyBindingsContent.SetActive(false);
        par_GameSettingsContent.SetActive(true);
    }
    //display all key bindings
    public void ShowKeyBindings()
    {
        btn_ShowKeyBindings.interactable = false;
        btn_ShowGameSettings.interactable = true;

        btn_Button1.onClick.RemoveAllListeners();
        btn_Button2.onClick.RemoveAllListeners();
        btn_Button3.onClick.RemoveAllListeners();
        btn_Button1.GetComponentInChildren<TMP_Text>().text = "General";
        btn_Button2.GetComponentInChildren<TMP_Text>().text = "Player";
        btn_Button3.GetComponentInChildren<TMP_Text>().text = "Combat";

        par_GameSettingsContent.SetActive(false);
        par_KeyBindingsContent.SetActive(true);
    }
    //display regular pause menu content
    public void ReturnToPauseMenu()
    {
        par_PM.SetActive(true);

        par_PMContent.SetActive(true);
        par_SettingsContent.SetActive(false);
        par_GameSettingsContent.SetActive(false);
        par_KeyBindingsContent.SetActive(false);
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