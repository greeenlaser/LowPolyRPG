using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Confirmation : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject par_ConfirmationUI;
    [SerializeField] private TMP_Text txt_Question;
    [SerializeField] private Button btn_Accept;
    [SerializeField] private Button btn_Decline;

    private void Start()
    {
        btn_Decline.onClick.AddListener(CloseUI);
    }

    //a simple accept or decline choice for UI
    public void UIConfirmationRequest(string callerScript,
                                      string callerMethod,
                                      string question)
    {
        if (SceneManager.GetActiveScene().buildIndex == 1
            && !GetComponent<UI_PauseMenu>().isPaused)
        {
            GetComponent<UI_PauseMenu>().PauseWithoutUI();
        }

        par_ConfirmationUI.SetActive(true);
        btn_Accept.onClick.RemoveAllListeners();
        txt_Question.text = question;

        if (callerScript == "mainMenu")
        {
            if (callerMethod == "quit")
            {
                btn_Accept.onClick.AddListener(delegate { GetComponent<UI_MainMenu>().Quit(); });
            }
        }
        else if (callerScript == "pauseMenu")
        {
            if (callerMethod == "returnToMM")
            {
                btn_Accept.onClick.AddListener(delegate { GetComponent<UI_PauseMenu>().LoadMainMenuScene(); });
            }
            else if (callerMethod == "quit")
            {
                btn_Accept.onClick.AddListener(delegate { GetComponent<UI_PauseMenu>().Quit(); });
            }
        }
    }

    public void CloseUI()
    {
        par_ConfirmationUI.SetActive(false);
    }
}