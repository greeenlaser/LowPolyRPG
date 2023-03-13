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
    [SerializeField] private Button btn_ApplyChoice;
    [SerializeField] private Button btn_ResetChoice;
    [SerializeField] private Transform par_UnusedSettings;
    [SerializeField] private GameObject par_Content;
    public List<GameObject> KeyCommands_General;
    public List<GameObject> KeyCommands_Movement;
    public List<GameObject> KeyCommands_Combat;
    public List<GameObject> Settings_General;
    public List<GameObject> Settings_Environment;
    public List<GameObject> Settings_Audio;

    //scripts
    private Manager_KeyBindings KeyBindingsScript;
    private Manager_Settings SettingsScript;

    private void Awake()
    {
        KeyBindingsScript = GetComponent<Manager_KeyBindings>();
        SettingsScript = GetComponent<Manager_Settings>();

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

        btn_ApplyChoice.onClick.RemoveAllListeners();
        btn_ResetChoice.onClick.RemoveAllListeners();

        btn_ApplyChoice.onClick.AddListener(KeyBindingsScript.SaveKeyBinds);
        btn_ResetChoice.onClick.AddListener(KeyBindingsScript.ResetKeyBinds);
    }
    public void SwitchToSettings()
    {
        btn_SwitchToKeyCommands.interactable = true;
        btn_SwitchToSettings.interactable = false;

        btn_Choice1.GetComponentInChildren<TMP_Text>().text = "General";
        btn_Choice2.GetComponentInChildren<TMP_Text>().text = "Graphics";
        btn_Choice3.GetComponentInChildren<TMP_Text>().text = "Audio";
        SwitchChoice(1);

        btn_ApplyChoice.onClick.RemoveAllListeners();
        btn_ResetChoice.onClick.RemoveAllListeners();

        btn_ApplyChoice.onClick.AddListener(SettingsScript.SaveSettings);
        btn_ApplyChoice.onClick.AddListener(SettingsScript.LoadSettings);
        btn_ResetChoice.onClick.AddListener(SettingsScript.ResetSettings);
    }
    public void SwitchChoice(int choice)
    {
        btn_Choice1.interactable = true;
        btn_Choice2.interactable = true;
        btn_Choice3.interactable = true;

        foreach (GameObject selectable in KeyCommands_General)
        {
            selectable.transform.SetParent(par_UnusedSettings.transform, false);
        }
        foreach (GameObject selectable in KeyCommands_Movement)
        {
            selectable.transform.SetParent(par_UnusedSettings.transform, false);
        }
        foreach (GameObject selectable in KeyCommands_Combat)
        {
            selectable.transform.SetParent(par_UnusedSettings.transform, false);
        }
        foreach (GameObject selectable in Settings_General)
        {
            selectable.transform.SetParent(par_UnusedSettings.transform, false);
        }
        foreach (GameObject selectable in Settings_Environment)
        {
            selectable.transform.SetParent(par_UnusedSettings.transform, false);
        }
        foreach (GameObject selectable in Settings_Audio)
        {
            selectable.transform.SetParent(par_UnusedSettings.transform, false);
        }

        if (choice == 1)
        {
            btn_Choice1.interactable = false;
            if (btn_Choice2.GetComponentInChildren<TMP_Text>().text == "Movement")
            {
                foreach (GameObject selectable in KeyCommands_General)
                {
                    selectable.transform.SetParent(par_Content.transform, false);
                }
            }
            else
            {
                foreach (GameObject selectable in Settings_General)
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
                foreach (GameObject selectable in KeyCommands_Movement)
                {
                    selectable.transform.SetParent(par_Content.transform, false);
                }
            }
            else
            {
                foreach (GameObject selectable in Settings_Environment)
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
                foreach (GameObject selectable in KeyCommands_Combat)
                {
                    selectable.transform.SetParent(par_Content.transform, false);
                }
            }
            else
            {
                foreach (GameObject selectable in Settings_Audio)
                {
                    selectable.transform.SetParent(par_Content.transform, false);
                }
            }
        }
    }
}