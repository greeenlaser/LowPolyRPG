using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class Manager_Settings : MonoBehaviour
{
    [Header("Assignables")]
    public List<UI_SettingsValue> settings = new();
    public List<AudioSource> sfx = new();
    public AudioSource currentSong;

    [Header("UI")]
    [SerializeField] private Button btn_ApplySettings;
    [SerializeField] private Button btn_ResetSettings;

    [Header("Scripts")]
    [SerializeField] private Volume volume;
    [SerializeField] private GameObject thePlayer;
    [SerializeField] private Camera playerCamera;

    //private variables
    private string settingsFilePath;
    private ColorAdjustments color;

    //scripts
    private GameManager GameManagerScript;
    private Manager_Console ConsoleScript;
    private Player_Camera PlayerCameraScript;

    private void Awake()
    {
        GameManagerScript = GetComponent<GameManager>();
        ConsoleScript = GetComponent<Manager_Console>();
        PlayerCameraScript = thePlayer.GetComponentInChildren<Camera>().GetComponent<Player_Camera>();

        volume.profile.TryGet(out color);

        btn_ApplySettings.onClick.AddListener(ApplySettings);
        btn_ResetSettings.onClick.AddListener(ResetSettings);
    }

    //resets all setting values to their default values
    public void ResetSettings()
    {
        foreach (UI_SettingsValue setting in settings)
        {
            setting.ResetValue();
            string settingType = setting.variableType.ToString();
            if (settingType == "isBool")
            {
                UpdateValue(setting.settingName, setting.settingValue_Bool_Default.ToString());
            }
            else if (settingType == "isFloat")
            {
                UpdateValue(setting.settingName, setting.settingValue_Number_Default.ToString());
            }
            else if (settingType == "isString")
            {
                UpdateValue(setting.settingName, setting.settingValue_String_Default);
            }
        }
        //delete settings file if settings are reset
        string[] files = Directory.GetFiles(GameManagerScript.settingsPath);
        foreach (string file in files)
        {
            if (file.Contains("SettingsFile.txt"))
            {
                File.Delete(file);
                break;
            }
        }
    }

    //applies all settings to changed settings if there are any changes
    //and saves all settings to external txt file
    public void ApplySettings()
    {
        //delete old settings file if settings are applied
        string[] files = Directory.GetFiles(GameManagerScript.settingsPath);
        foreach (string file in files)
        {
            if (file.Contains("SettingsFile.txt"))
            {
                File.Delete(file);
                break;
            }
        }

        //using a text editor to write new text to new debug file in the debug file path
        using StreamWriter settingsFile = File.CreateText(settingsFilePath);

        settingsFile.WriteLine("Settings file for Low_poly_RPG.");
        settingsFile.WriteLine("");

        foreach (UI_SettingsValue setting in settings)
        {
            string settingType = setting.variableType.ToString();

            if (settingType == "isBool")
            {
                if (setting.settingValue_Bool 
                    != setting.settingValue_Bool_Default)
                {
                    Debug.Log(setting.settingName + ", " + setting.toggle.isOn.ToString() + ", " + setting.settingValue_Bool.ToString());

                    setting.UpdateValue(setting.toggle.isOn.ToString());
                    UpdateValue(setting.settingName, setting.settingValue_Bool.ToString());

                    Debug.Log(setting.settingName + ", " + setting.toggle.isOn.ToString() + ", " + setting.settingValue_Bool.ToString());
                }
                settingsFile.WriteLine(setting.settingName + ": " + setting.settingValue_Bool.ToString());
            }
            else if (settingType == "isFloat")
            {
                if (setting.settingValue_Number
                    != setting.settingValue_Number_Default)
                {
                    setting.UpdateValue(setting.slider.value.ToString());
                    UpdateValue(setting.settingName, setting.settingValue_Number.ToString());
                }
                settingsFile.WriteLine(setting.settingName + ": " + setting.settingValue_Number.ToString());
            }
            else if (settingType == "isString")
            {
                if (setting.settingValue_String
                    != setting.settingValue_String_Default)
                {
                    setting.UpdateValue(setting.settingValue_String);
                    UpdateValue(setting.settingName, setting.settingValue_String);
                }
                settingsFile.WriteLine(setting.settingName + ": " + setting.settingValue_String);
            }
        }
    }

    //loads all settings from the settings file
    public void LoadSettings()
    {
        settingsFilePath = GameManagerScript.settingsPath + @"\SettingsFile.txt";

        if (!File.Exists(settingsFilePath))
        {
            ResetSettings();

            ConsoleScript.CreateNewConsoleLine("Loaded default settings.", "FILE NOT FOUND");
        }
        else
        {
            foreach (string line in File.ReadLines(settingsFilePath))
            {
                if (line.Contains(':'))
                {
                    string[] valueSplit = line.Split(':');
                    string type = valueSplit[0];
                    string value = valueSplit[1].Replace(" ", string.Empty);

                    foreach (UI_SettingsValue setting in settings)
                    {
                        if (type == setting.settingName)
                        {
                            if (!Regex.IsMatch(value, @"-?\d+")
                                && setting.variableType.ToString() != "isBool")
                            {
                                ConsoleScript.CreateNewConsoleLine("Error: Settings file value for " + type + " cannot be " + value + "! Skipping and resetting to default value.", "INVALID_VARIABLE");
                                setting.ResetValue();
                            }
                            else
                            {
                                string settingType = setting.variableType.ToString();
                                if (settingType == "isBool")
                                {
                                    setting.settingValue_Bool = value == "True";
                                }
                                else if (settingType == "isFloat")
                                {
                                    setting.settingValue_Number = float.Parse(value);
                                }
                                else if (settingType == "isString")
                                {
                                    setting.settingValue_String = value;
                                }
                            }
                            break;
                        }
                    }
                }
            }

            ApplySettings();

            ConsoleScript.CreateNewConsoleLine("Success: Loaded settings file.", "FILE LOAD SUCCESS");
        }
    }

    //this method changes the actual in-game setting value to the desired choice
    //once all checks have passed that this setting is allowed and settings are applied
    public void UpdateValue(string setting, string value)
    {
        //general
        if (setting == "fov")
        {
            playerCamera.fieldOfView = float.Parse(value);
        }
        else if (setting == "mousespeed")
        {
            PlayerCameraScript.sensX = float.Parse(value);
            PlayerCameraScript.sensY = float.Parse(value);
        }

        //graphics
        else if (setting == "vsync")
        {
            if (value == "True")
            {
                Application.targetFrameRate = 60;
            }
            else
            {
                Application.targetFrameRate = 999;
            }
        }
        else if (setting == "resolution")
        {
            string[] valueSplit = value.Split('x');
            int res1 = int.Parse(valueSplit[0]);
            int res2 = int.Parse(valueSplit[1]);

            Screen.SetResolution(res1, res2, true);
        }
        else if (setting == "brightness")
        {
            float finalVal = float.Parse(value) / 10;
            color.postExposure.value = finalVal;
        } 
        else if (setting == "contrast")
        {
            color.contrast.value = float.Parse(value);
        }
        else if (setting == "colorblindmode")
        {

        }

        //audio
        else if (setting == "mastervolume")
        {
            
        }
        else if (setting == "musicvolume")
        {
            float finalVal = float.Parse(value) / 100;
            currentSong.volume = finalVal;
        }
        else if (setting == "sfxvolume")
        {
            foreach (AudioSource sfx in sfx)
            {
                float finalVal = float.Parse(value) / 100;
                sfx.volume = finalVal;
            }
        }
    }
}
