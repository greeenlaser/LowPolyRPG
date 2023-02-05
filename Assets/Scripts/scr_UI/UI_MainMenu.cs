using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject par_MMContent;
    [SerializeField] private GameObject par_CreditsContent;
    [SerializeField] private Button btn_Play;
    [SerializeField] private Button btn_Credits;
    [SerializeField] private Button btn_ReturnToMM;
    [SerializeField] private Button btn_Quit;

    private void Start()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        btn_Play.onClick.AddListener(LoadGameScene);
        btn_Credits.onClick.AddListener(OpenCredits);
        btn_ReturnToMM.onClick.AddListener(OpenMM);
        btn_Quit.onClick.AddListener( delegate { GetComponent<UI_Confirmation>().
            UIConfirmationRequest("mainMenu",
                                  "quit",
                                  "Do you want to go quit the game?"); });
    }

    //switch to game scene
    public void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }

    //display all main menu content
    public void OpenMM()
    {
        par_CreditsContent.SetActive(false);
        par_MMContent.SetActive(true);
    }
    //display all credits menu content
    public void OpenCredits()
    {
        par_MMContent.SetActive(false);
        par_CreditsContent.SetActive(true);
    }

    //quit the game
    public void Quit()
    {
        Application.Quit();
    }
}