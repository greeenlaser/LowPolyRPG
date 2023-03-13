using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Manager_KeyBindings : MonoBehaviour
{
    [Header("Assignables")]
    public List<UI_KeyBindingValue> KeyBindings = new();

    [Header("UI")]
    [SerializeField] private GameObject par_AssignUI;
    [SerializeField] private TMP_Text txt_AssignTitle;

    public readonly string[] keycodes = new[]
    {
        "None", "Backspace", "Tab", "Clear", "Return", "Pause", "Escape", "Space", "Exclaim", "DoubleQuote", "Hash", "Dollar", "Ampersand",
        "Quote", "LeftParen", "RightParen", "Asterisk", "Plus", "Comma", "Minus", "Period", "Slash",
        "Alpha0", "Alpha1", "Alpha2", "Alpha3", "Alpha4", "Alpha5", "Alpha6", "Alpha7", "Alpha8", "Alpha9",
        "Colon", "Semicolon", "Less", "Equals", "Greater", "Question", "At", "LeftBracket", "Backslash", "RightBracket", "Caret", "Underscore", "BackQuote",
        "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
        "Delete", "Keypad0", "Keypad1", "Keypad2", "Keypad3", "Keypad4", "Keypad5", "Keypad6", "Keypad7", "Keypad8", "Keypad9",
        "KeypadPeriod", "KeypadDivide", "KeypadMultiply", "KeypadMinus",  "KeypadPlus", "KeypadEnter", "KeypadEquals",
        "UpArrow", "DownArrow", "RightArrow", "LeftArrow", "Insert", "Home", "End", "PageUp", "PageDown",
        "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12", "F13", "F14", "F15",
        "Numlock", "CapsLock", "ScrollLock", "RightShift", "LeftShift", "RightControl", "LeftControl", "RightAlt", "LeftAlt",
        "RightCommand", "LeftCommand", "LeftWindows", "RightWindows", "AltGr", "Help", "Print", "SysReq", "Break", "Menu",
        "Mouse0", "Mouse1", "Mouse2", "Mouse3", "Mouse4", "Mouse5", "Mouse6"
    };

    //private variables
    private string keyBindsFilePath;
    private string key;
    private float timer;

    //scripts
    private GameManager GameManagerScript;
    private UI_PauseMenu PauseMenuScript;
    private Manager_Console ConsoleScript;

    private void Awake()
    {
        GameManagerScript = GetComponent<GameManager>();
        PauseMenuScript = GetComponent<UI_PauseMenu>();
        ConsoleScript = GetComponent<Manager_Console>();
    }

    private void Update()
    {
        if (PauseMenuScript.isChangingKey)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                StopKeyAssign();
            }
        }
    }

    private void OnGUI()
    {
        if (PauseMenuScript.isChangingKey)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                KeyCode pressedKey = e.keyCode;
                if (pressedKey != KeyCode.None)
                {
                    CheckForValidKey(pressedKey);
                }
            }
        }
    }

    //this function opens the key assign ui and starts the key assigning
    public void StartKeyAssign(string keyName)
    {
        key = keyName;
        timer = 5;

        par_AssignUI.SetActive(true);
        txt_AssignTitle.text = "Press any key to assign it to " + key;

        PauseMenuScript.isChangingKey = true;
    }
    //check if pressed key is allowed and then assign new keycode to selected key
    public void CheckForValidKey(KeyCode pressedKey)
    {
        string thePressedKey = pressedKey.ToString();

        bool foundCorrectKey = false;
        foreach (string key in keycodes)
        {
            if (thePressedKey == key)
            {
                foundCorrectKey = true;
                break;
            }
        }

        if (!foundCorrectKey)
        {
            ConsoleScript.CreateNewConsoleLine("Error: Pressed key " + thePressedKey + " is not a valid key!", "KEY ASSIGN FAIL");
        }
        else
        {
            foreach (UI_KeyBindingValue keyBind in KeyBindings)
            {
                if (keyBind.keyBindName == key)
                {
                    keyBind.keyBindValue = thePressedKey;
                    keyBind.txt_ButtonText.text = thePressedKey;

                    PauseMenuScript.isChangingKey = false;
                    par_AssignUI.SetActive(false);

                    break;
                }
            }
        }
    }

    //this method is only ran if the player did not assign a key in 5 seconds
    //it will close the assign ui and no key is assigned to selected key
    private void StopKeyAssign()
    {
        PauseMenuScript.isChangingKey = false;
        par_AssignUI.SetActive(false);
        ConsoleScript.CreateNewConsoleLine("Error: Did not assign any key to " + key + " because key assign timer ran out.", "KEY ASSIGN FAIL" );
    }

    //reset keybinds to their default values
    public void ResetKeyBinds()
    {
        foreach (UI_KeyBindingValue keyBind in KeyBindings)
        {
            keyBind.keyBindValue = keyBind.keyBindValue_Default;
            keyBind.txt_ButtonText.text = keyBind.keyBindValue;
        }
    }

    //save keybinds to an external txt file
    public void SaveKeyBinds()
    {
        //delete old key binds file if key binds are applied
        string[] files = Directory.GetFiles(GameManagerScript.settingsPath);
        foreach (string file in files)
        {
            if (file.Contains("KeyBindsFile.txt"))
            {
                File.Delete(file);
                break;
            }
        }

        using StreamWriter settingsFile = File.CreateText(keyBindsFilePath);

        settingsFile.WriteLine("Key binds file for Low_poly_RPG.");
        settingsFile.WriteLine("");

        foreach (UI_KeyBindingValue keyBind in KeyBindings)
        {
            settingsFile.WriteLine(keyBind.keyBindName + ": " + keyBind.keyBindValue);
        }

        ConsoleScript.CreateNewConsoleLine("Successfully saved all key binds to file.", "FILE SAVE SUCCESS");
    }

    //load keybinds from an external txt file
    public void LoadKeyBinds() 
    {
        keyBindsFilePath = GameManagerScript.settingsPath + @"\KeyBindsFile.txt";

        if (!File.Exists(keyBindsFilePath))
        {
            ResetKeyBinds();

            ConsoleScript.CreateNewConsoleLine("Loaded default key binds.", "FILE NOT FOUND");
        }
        else
        {
            foreach (string line in File.ReadLines(keyBindsFilePath))
            {
                if (line.Contains(':'))
                {
                    string[] valueSplit = line.Split(':');
                    string type = valueSplit[0];
                    string value = valueSplit[1].Replace(" ", string.Empty);

                    foreach (UI_KeyBindingValue keyBind in KeyBindings)
                    {
                        if (type == keyBind.keyBindName)
                        {
                            bool foundKey = false;
                            foreach (string allowedKey in keycodes)
                            {
                                if (value == allowedKey)
                                {
                                    foundKey = true;
                                    break;
                                }
                            }

                            if (!foundKey)
                            {
                                keyBind.keyBindValue = keyBind.keyBindValue_Default;
                                keyBind.txt_ButtonText.text = keyBind.keyBindValue;

                                ConsoleScript.CreateNewConsoleLine("Error: Key binds file value for " + type + " cannot be " + value + "! Skipping and resetting to default value.", "INVALID_VARIABLE");
                            }
                            else
                            {
                                keyBind.keyBindValue = value;
                                keyBind.txt_ButtonText.text = value;
                            }
                        }
                    }
                }
            }

            ConsoleScript.CreateNewConsoleLine("Successfully loaded key binds file.", "FILE LOAD SUCCESS");
        }
    }
}