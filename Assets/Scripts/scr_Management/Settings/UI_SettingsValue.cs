using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SettingsValue : MonoBehaviour
{
    [Header("Variable setup")]
    [SerializeField] private string settingName;
    [SerializeField] private VariableType variableType;
    [SerializeField] private enum VariableType
    {
        isBool,
        isFloat,
        isString
    }

    [Header("Variable default value")]
    [SerializeField] private float settingValue_Number_Default;
    [SerializeField] private bool settingValue_Bool_Default;
    [SerializeField] private string settingValue_String_Default;
    [SerializeField] private List<string> settingValue_String_Choices;

    [Header("Variable UI")]
    [SerializeField] private Toggle toggle;
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Dropdown dropdown;

    //current variable value
    [HideInInspector] public float settingValue_Number;
    [HideInInspector] public bool settingValue_Bool;
    [HideInInspector] public string settingValue_String;

    //scripts
    private Manager_Console ConsoleScript;

    private void Awake()
    {
        ConsoleScript = FindObjectOfType<Manager_Console>();

        if (dropdown != null)
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(settingValue_String_Choices);
            foreach (string choice in settingValue_String_Choices)
            {
                dropdown.onValueChanged.AddListener(delegate { UpdateSelectedValue(); }) ;
            }
        }
    }

    //returns current value of this setting to caller
    public string GetCurrentValue()
    {
        return variableType switch
        {
            VariableType.isBool => settingValue_Bool.ToString(),
            VariableType.isFloat => settingValue_Number.ToString(),
            VariableType.isString => settingValue_String,
            _ => ""
        };
    }
    //updates current value of this setting to new selected value
    public void UpdateValue(string insertedValue)
    {
        switch (variableType)
        {
            case VariableType.isBool:
                settingValue_Bool = insertedValue == "True";
                toggle.isOn = settingValue_Bool;
                break;
            case VariableType.isFloat:
                float value = float.Parse(insertedValue);
                if (value >= slider.minValue
                    && value <= slider.maxValue)
                {
                    settingValue_Number = float.Parse(insertedValue);
                    slider.value = settingValue_Number;
                }
                else
                {
                    ConsoleScript.CreateNewConsoleLine(
                        "Error: Inserted value " + insertedValue + " " +
                        "for " + settingName + " is out of range!", "INVALID_VARIABLE_VALUE");
                }
                break;
            case VariableType.isString:
                bool foundValue = false;
                foreach (string choice in settingValue_String_Choices)
                {
                    if (insertedValue == choice)
                    {
                        foundValue = true;
                        break;
                    }
                }

                if (foundValue)
                {
                    settingValue_String = insertedValue;
                    for (int i = 0; i < settingValue_String_Choices.Count; i++)
                    {
                        string selectedValue = settingValue_String_Choices[i];
                        string userDefinedValue = settingValue_String;

                        if (userDefinedValue == selectedValue)
                        {
                            dropdown.value = i;
                            break;
                        }
                    }
                }
                else
                {
                    ConsoleScript.CreateNewConsoleLine(
                        "Error: Inserted value " + insertedValue + " " +
                        "for " + settingName + " is not valid!", "INVALID_VARIABLE_VALUE");
                }
                break;
        }
    }
    //resets current value of this setting to original value
    public void ResetValue()
    {
        switch (variableType)
        {
            case VariableType.isBool:
                settingValue_Bool = settingValue_Bool_Default;
                toggle.isOn = settingValue_Bool;
                break;
            case VariableType.isFloat:
                settingValue_Number = settingValue_Number_Default;
                slider.value = settingValue_Number;
                break;
            case VariableType.isString:
                settingValue_String = settingValue_String_Default;
                for (int i = 0; i < settingValue_String_Choices.Count; i++)
                {
                    string selectedValue = settingValue_String_Choices[i];
                    string userDefinedValue = settingValue_String;

                    if (userDefinedValue == selectedValue)
                    {
                        dropdown.value = i;
                        break;
                    }
                }
                break;
        }
    }

    //this event is called to change current selected value
    //to new selected value whenever the current dropdown choice is updated
    public void UpdateSelectedValue()
    {
        settingValue_String = dropdown.value.ToString();
        Debug.Log(settingValue_String);
    }
}