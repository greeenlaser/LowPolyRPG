using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Assignables")]
    [SerializeField] private GameObject thePlayer;

    //public but hidden variables
    [HideInInspector] public string parentPath;
    [HideInInspector] public string gamePath;
    [HideInInspector] public string savePath;
    [HideInInspector] public string settingsPath;
    [HideInInspector] public string debugFilePath;

    //scripts
    private UI_PauseMenu PauseMenuScript;
    private Manager_Console ConsoleScript;
    private Player_Movement PlayerMovementScript;

    //private variables
    private int currentScene;

    private void Awake()
    {
        PauseMenuScript = GetComponent<UI_PauseMenu>();
        ConsoleScript = GetComponent<Manager_Console>();
        PlayerMovementScript = thePlayer.GetComponent<Player_Movement>();

        currentScene = SceneManager.GetActiveScene().buildIndex;

        //start recieving unity logs
        Application.logMessageReceived += GetComponent<Manager_Console>().HandleLog;

        parentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\My Games";
        gamePath = parentPath + @"\Low_poly_RPG";
        savePath = gamePath + @"\Game saves";
        settingsPath = gamePath + @"\Settings";

        //create game directories
        CreatePaths();

        //get debug file path
        DirectoryInfo dir = new(gamePath);
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            if (file.Name.Contains("DebugFile_"))
            {
                debugFilePath = file.FullName;
                break;
            }
        }

        //always recreates the debug log in main menu scene,
        //only recreates the debug log in game scene if user is in engine
        if (currentScene == 0
            || (currentScene == 1
            && Application.isEditor))
        {
            //create debug file
            CreateDebugFile();
        }
    }

    private void Start()
    {
        PauseMenuScript.UnpauseGame();
        ConsoleScript.CloseConsole();

        PlayerMovementScript.LoadPlayer();
    }

    public void CreatePaths()
    {
        //create My Games folder
        if (!Directory.Exists(parentPath))
        {
            Directory.CreateDirectory(parentPath);
        }
        //create game folder
        if (!Directory.Exists(gamePath))
        {
            Directory.CreateDirectory(gamePath);
        }
        //create save path folder
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        //create settings path folder
        if (!Directory.Exists(settingsPath))
        {
            Directory.CreateDirectory(settingsPath);
        }
    }

    //creates the debug file
    public void CreateDebugFile()
    {
        //delete old debug file if player switched to main menu scene
        string[] files = Directory.GetFiles(gamePath);
        foreach (string file in files)
        {
            if (file.Contains("DebugFile_"))
            {
                File.Delete(file);
                break;
            }
        }

        string date = DateTime.Now.ToString();
        string replaceSlash = date.Replace('/', '_');
        string replaceColon = replaceSlash.Replace(':', '_');
        string replaceEmpty = replaceColon.Replace(' ', '_');
        debugFilePath = gamePath + @"\DebugFile_" + replaceEmpty + ".txt";

        //using a text editor to write new text to new debug file in the debug file path
        using StreamWriter debugFile = File.CreateText(debugFilePath);

        debugFile.WriteLine("Debug information file for Low_poly_RPG.");
        debugFile.WriteLine("");

        //add user cpu
        string processorType = SystemInfo.processorType;
        int processorThreadCount = SystemInfo.processorCount;
        int processorFrequency = SystemInfo.processorFrequency;

        debugFile.WriteLine("CPU: " + processorType + "with " + processorThreadCount + " threads at " + processorFrequency + "mhz");
        //add user gpu
        string gpuName = SystemInfo.graphicsDeviceName;
        int gpuMemory = SystemInfo.graphicsMemorySize / 1000;

        debugFile.WriteLine("GPU: " + gpuName + " with " + gpuMemory + "gb memory");
        //add user ram
        int ramSize = SystemInfo.systemMemorySize / 1000;

        debugFile.WriteLine("RAM: " + ramSize + "gb");
        //add user OS
        string osVersion = SystemInfo.operatingSystem;

        debugFile.WriteLine("OS: " + osVersion);

        debugFile.WriteLine("");
    }
}