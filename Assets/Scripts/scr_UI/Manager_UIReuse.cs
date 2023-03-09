using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Manager_UIReuse : MonoBehaviour
{
    [Header("Settings and key commands")]
    public GameObject par_SettingsParent;
    [SerializeField] private Button btn_SwitchToKeyCommands;
    [SerializeField] private Button btn_SwitchToSettings;
    [SerializeField] private Button btn_Choice1;
    [SerializeField] private Button btn_Choice2;
    [SerializeField] private Button btn_Choice3;
    [SerializeField] private Transform par_UnusedSettings;
    [SerializeField] private GameObject par_Content;
    public List<GameObject> choice1_KeyCommands;
    public List<GameObject> choice2_KeyCommands;
    public List<GameObject> choice3_KeyCommands;
    public List<GameObject> choice1_Settings;
    public List<GameObject> choice2_Settings;
    public List<GameObject> choice3_Settings;

    private void Awake()
    {
        btn_SwitchToKeyCommands.onClick.AddListener(SwitchToKeyCommands);
        btn_SwitchToSettings.onClick.AddListener(SwitchToSettings);
        btn_Choice1.onClick.AddListener(delegate { SwitchChoice(1); });
        btn_Choice2.onClick.AddListener(delegate { SwitchChoice(2); });
        btn_Choice3.onClick.AddListener(delegate { SwitchChoice(3); });
    }

    public void SwitchToKeyCommands()
    {
        btn_SwitchToKeyCommands.interactable = false;
        btn_SwitchToSettings.interactable = true;

        btn_Choice1.GetComponentInChildren<TMP_Text>().text = "General";
        btn_Choice2.GetComponentInChildren<TMP_Text>().text = "Movement";
        btn_Choice3.GetComponentInChildren<TMP_Text>().text = "Combat";
        SwitchChoice(1);
    }
    public void SwitchToSettings()
    {
        btn_SwitchToKeyCommands.interactable = true;
        btn_SwitchToSettings.interactable = false;

        btn_Choice1.GetComponentInChildren<TMP_Text>().text = "General";
        btn_Choice2.GetComponentInChildren<TMP_Text>().text = "Graphics";
        btn_Choice3.GetComponentInChildren<TMP_Text>().text = "Audio";
        SwitchChoice(1);
    }
    public void SwitchChoice(int choice)
    {
        btn_Choice1.interactable = true;
        btn_Choice2.interactable = true;
        btn_Choice3.interactable = true;

        foreach (GameObject selectable in choice1_KeyCommands)
        {
            selectable.transform.SetParent(par_UnusedSettings.transform, false);
        }
        foreach (GameObject selectable in choice2_KeyCommands)
        {
            selectable.transform.SetParent(par_UnusedSettings.transform, false);
        }
        foreach (GameObject selectable in choice3_KeyCommands)
        {
            selectable.transform.SetParent(par_UnusedSettings.transform, false);
        }
        foreach (GameObject selectable in choice1_Settings)
        {
            selectable.transform.SetParent(par_UnusedSettings.transform, false);
        }
        foreach (GameObject selectable in choice2_Settings)
        {
            selectable.transform.SetParent(par_UnusedSettings.transform, false);
        }
        foreach (GameObject selectable in choice3_Settings)
        {
            selectable.transform.SetParent(par_UnusedSettings.transform, false);
        }

        if (choice == 1)
        {
            btn_Choice1.interactable = false;
            if (btn_Choice2.GetComponentInChildren<TMP_Text>().text == "Movement")
            {
                foreach (GameObject selectable in choice1_KeyCommands)
                {
                    selectable.transform.SetParent(par_Content.transform, false);
                }
            }
            else
            {
                foreach (GameObject selectable in choice1_Settings)
                {
                    selectable.transform.SetParent(par_Content.transform, false);
                }
            }
        }
        else if (choice == 2)
        {
            btn_Choice2.interactable = false;
            if (btn_Choice2.GetComponentInChildren<TMP_Text>().text == "Movement")
            {
                foreach (GameObject selectable in choice2_KeyCommands)
                {
                    selectable.transform.SetParent(par_Content.transform, false);
                }
            }
            else
            {
                foreach (GameObject selectable in choice2_Settings)
                {
                    selectable.transform.SetParent(par_Content.transform, false);
                }
            }
        }
        else if (choice == 3)
        {
            btn_Choice3.interactable = false;
            if (btn_Choice2.GetComponentInChildren<TMP_Text>().text == "Movement")
            {
                foreach (GameObject selectable in choice3_KeyCommands)
                {
                    selectable.transform.SetParent(par_Content.transform, false);
                }
            }
            else
            {
                foreach (GameObject selectable in choice3_Settings)
                {
                    selectable.transform.SetParent(par_Content.transform, false);
                }
            }
        }
    }
}