using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using UnityEditor;
using UnityEngine;

//check if string only contains upper or lowercase english alphabet letters
//Regex.IsMatch("", @"^[a-zA-Z]+$")
//check if string only contains positive or negative ints
//Regex.IsMatch("", @"-?\d+")
//check if string only contains positive or negative ints or floats
//Regex.IsMatch("", @"-?\d*\.?\d*")

public class Manager_Console : MonoBehaviour
{
    [Header("UI")]
    public GameObject par_Console;
    [SerializeField] private TMP_InputField consoleInputField;
    [SerializeField] private GameObject par_ConsoleContent;
    [SerializeField] private TMP_Text txt_InsertedTextTemplate;

    [Header("Other assignables")]
    [SerializeField] private GameObject par_Managers;

    //scripts
    private GameManager GameManagerScript;
    private UI_PauseMenu PauseMenuScript;
    private UI_DebugMenu DebugMenuScript;

    //private variables
    private bool debugMenuEnabled = true;
    private bool startedConsoleSetupWait;
    private string input;
    private string output;
    private string lastOutput;
    private int currentSelectedInsertedCommand;
    private readonly List<string> separatedWords = new();
    private readonly List<GameObject> createdTexts = new();
    private readonly List<string> insertedCommands = new();

    public enum MessageType
    {
        CONSOLE_INFO_MESSAGE,
        CONSOLE_SUCCESS_MESSAGE,
        CONSOLE_ERROR_MESSAGE,
        CONSOLE_COMMAND,
        UNITY_LOG_MESSAGE,
        UNITY_LOG_ERROR,
        INVALID_FILE_VARIABLE_VALUE,
        UNAUTHORIZED_PLAYER_ACTION
    }

    private void Awake()
    {
        GameManagerScript = GetComponent<GameManager>();
        PauseMenuScript = GetComponent<UI_PauseMenu>();
        DebugMenuScript = GetComponent<UI_DebugMenu>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            PauseMenuScript.isConsoleOpen = !PauseMenuScript.isConsoleOpen;
        }

        if (PauseMenuScript.isConsoleOpen
            && !par_Console.activeInHierarchy)
        {
            OpenConsole();
        }
        else if (!PauseMenuScript.isConsoleOpen
                 && par_Console.activeInHierarchy)
        {
            CloseConsole();
        }

        //choose newer and older inserted commands with arrow keys
        if (insertedCommands.Count > 0
            && PauseMenuScript.isConsoleOpen)
        {
            //always picks newest added console command if input field is empty
            if ((Input.GetKeyDown(KeyCode.UpArrow)
                || Input.GetKeyDown(KeyCode.DownArrow))
                && consoleInputField.text == "")
            {
                currentSelectedInsertedCommand = insertedCommands.Count - 1;
                consoleInputField.text = insertedCommands[^1];
                consoleInputField.MoveToEndOfLine(false, false);
            }
            else
            {
                //choose older typed command
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    currentSelectedInsertedCommand--;
                    if (currentSelectedInsertedCommand < 0)
                    {
                        currentSelectedInsertedCommand = insertedCommands.Count - 1;
                    }

                    consoleInputField.text = insertedCommands[currentSelectedInsertedCommand];
                    consoleInputField.MoveToEndOfLine(false, false);
                }
                //choose newer typed command
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    currentSelectedInsertedCommand++;
                    if (currentSelectedInsertedCommand == insertedCommands.Count)
                    {
                        currentSelectedInsertedCommand = 0;
                    }

                    consoleInputField.text = insertedCommands[currentSelectedInsertedCommand];
                    consoleInputField.MoveToEndOfLine(false, false);
                }
            }
        }
    }

    private void OpenConsole()
    {
        if (!PauseMenuScript.isPaused)
        {
            PauseMenuScript.PauseWithoutUI();
        }

        PauseMenuScript.isConsoleOpen = true;
        par_Console.SetActive(true);
        consoleInputField.ActivateInputField();
    }
    public void CloseConsole()
    {
        PauseMenuScript.isConsoleOpen = false;
        par_Console.SetActive(false);

        if (!PauseMenuScript.par_PM.activeInHierarchy)
        {
            PauseMenuScript.UnpauseGame();
        }
    }

    public void CheckInsertedText()
    {
        //splits each word as its own and adds to separated words list
        foreach (string word in input.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            separatedWords.Add(word);
        }

        insertedCommands.Add(input);
        currentSelectedInsertedCommand = insertedCommands.Count - 1;
        CreateNewConsoleLine("--" + input + "--", MessageType.CONSOLE_COMMAND.ToString());

        //if inserted text was not empty and player pressed enter
        if (separatedWords.Count >= 1)
        {
            if (Regex.IsMatch(separatedWords[0], @"-?\d+"))
            {
                CreateNewConsoleLine("Error: Console command cannot start with a number!", MessageType.CONSOLE_ERROR_MESSAGE.ToString());
            }
            else
            {
                //clear console log
                if (separatedWords[0] == "clear"
                         && separatedWords.Count == 1)
                {
                    Command_ClearConsole();
                }
                //toggle debug menu
                else if (separatedWords[0] == "tdm"
                         && separatedWords.Count == 1)
                {
                    Command_ToggleDebugMenu();
                }
                //quit game
                else if (separatedWords[0] == "quit"
                         && separatedWords.Count == 1)
                {
                    Command_ForceQuit();
                }
                     
                else
                {
                    insertedCommands.Add(input);
                    currentSelectedInsertedCommand = insertedCommands.Count - 1;

                    CreateNewConsoleLine("Error: Unknown or invalid command!", MessageType.CONSOLE_ERROR_MESSAGE.ToString());
                }
            }
        }
        else
        {
            insertedCommands.Add(input);
            currentSelectedInsertedCommand = insertedCommands.Count - 1;

            CreateNewConsoleLine("Error: No command was inserted!", MessageType.CONSOLE_ERROR_MESSAGE.ToString());
        }

        separatedWords.Clear();

        input = "";
    }

    //clear console log
    private void Command_ClearConsole()
    {
        separatedWords.Clear();

        lastOutput = "";

        foreach (GameObject createdText in createdTexts)
        {
            Destroy(createdText);
        }
        createdTexts.Clear();
    }
    //toggle debug menu
    public void Command_ToggleDebugMenu()
    {
        if (!debugMenuEnabled)
        {
            DebugMenuScript.par_DebugMenu.transform.position -= new Vector3(0, 114, 0);

            CreateNewConsoleLine("Enabled debug menu.", MessageType.CONSOLE_INFO_MESSAGE.ToString());
            debugMenuEnabled = true;
        }
        else
        {
            DebugMenuScript.par_DebugMenu.transform.position += new Vector3(0, 114, 0);

            CreateNewConsoleLine("Disabled debug menu.", MessageType.CONSOLE_INFO_MESSAGE.ToString());
            debugMenuEnabled = false;
        }
    }
    //force-quit game
    public void Command_ForceQuit()
    {
        CreateNewConsoleLine("Force-quit game through console.", MessageType.CONSOLE_INFO_MESSAGE.ToString());

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif

#if UNITY_STANDALONE
        Application.Quit();
#endif
    }

    //reads all unity debug log messages and creates a new debug file message for each one
    public void HandleLog(string logString, string unusedStackString, LogType type)
    {
        if (par_Managers != null)
        {
            output = logString;

            if (!startedConsoleSetupWait)
            {
                if (lastOutput == output)
                {
                    startedConsoleSetupWait = true;
                    StartCoroutine(ConsoleSetupWait());
                }

                NewUnitylogMessage(unusedStackString, type);
            }
        }
    }

    //reads inserted text from input field in console UI
    public void ReadStringInput(string s)
    {
        input = s;
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CheckInsertedText();
            consoleInputField.text = "";
            consoleInputField.ActivateInputField();
        }
    }

    //creates a new debug file line with time, info type and message
    private void NewUnitylogMessage(string unusedStackString, LogType type)
    {
        string resultMessage;

        if (output.Contains("Exception")
            || output.Contains("CS")
            || output.Contains("Error"))
        {
            resultMessage = MessageType.UNITY_LOG_ERROR.ToString() + "] [" + unusedStackString + "] [" + type + "]";
        }
        else if (output.Contains("Incorrect value"))
        {
            resultMessage = MessageType.INVALID_FILE_VARIABLE_VALUE.ToString();
        }
        else if (output.Contains("Requirements not met"))
        {
            resultMessage = MessageType.UNAUTHORIZED_PLAYER_ACTION.ToString();
        }
        else
        {
            resultMessage = MessageType.UNITY_LOG_MESSAGE.ToString();
        }

        CreateNewConsoleLine(output, resultMessage);
        lastOutput = output;
    }

    //add a new line to the console
    public void CreateNewConsoleLine(string message, string source)
    {
        if (par_Managers != null)
        {
            GameObject newConsoleText = Instantiate(txt_InsertedTextTemplate.gameObject);
            createdTexts.Add(newConsoleText);

            //check if createdTexts list is longer than limit
            //and remove oldest
            if (createdTexts.Count > 200)
            {
                GameObject oldestText = createdTexts[0];
                createdTexts.Remove(oldestText);
                Destroy(oldestText);
            }

            string date = "[" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "]";
            string msg = date + " [" + source.Replace("_", " ") + "] - " + message + "\n";

            newConsoleText.transform.SetParent(par_ConsoleContent.transform, false);
            newConsoleText.GetComponent<TMP_Text>().text = date + " " + message;

            using StreamWriter debugFile = File.AppendText(GameManagerScript.debugFilePath);

            if (message != "")
            {
                debugFile.WriteLine(msg);
            }
        }
    }

    //waits half a second when game starts before allowing console to enable
    private IEnumerator ConsoleSetupWait()
    {
        yield return new WaitForSeconds(0.5f);
        startedConsoleSetupWait = false;
    }
}