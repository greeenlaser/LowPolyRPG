using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SettingsValue : MonoBehaviour
{
    [Header("Variable setup")]
    public string settingName;
    public VariableType variableType;
    public enum VariableType
    {
        isBool,
        isFloat,
        isString
    }

    [Header("Bool")]
    public bool settingValue_Bool_Default;
    public Toggle toggle;
    [HideInInspector] public bool settingValue_Bool;

    [Header("Number")]
    public float settingValue_Number_Default;
    public Slider slider;
    [SerializeField] private TMP_Text txt_SliderValue;
    [HideInInspector] public float settingValue_Number;

    [Header("Choice")]
    public string settingValue_String_Default;
    public List<string> settingValue_String_Choices;
    public TMP_Dropdown dropdown;
    [HideInInspector] public string settingValue_String;

    private void Awake()
    {
        if (toggle != null)
        {
            toggle.onValueChanged.AddListener(delegate { UpdateToggle(); });
        }
        if (slider != null)
        {
            slider.onValueChanged.AddListener(delegate { UpdateSlider(); });
        }
        if (dropdown != null)
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(settingValue_String_Choices);
            foreach (string choice in settingValue_String_Choices)
            {
                dropdown.onValueChanged.AddListener(delegate { UpdateValue(choice); }) ;
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

    //assigned to the toggle of this setting
    public void UpdateToggle()
    {
        UpdateValue(toggle.isOn.ToString());
    }
    //assigned to the slider of this setting
    public void UpdateSlider()
    {
        UpdateValue(slider.value.ToString());
    }
    //assigned to the dropdown of this setting
    public void UpdateDropdown()
    {

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
                    txt_SliderValue.text = settingValue_Number.ToString();
                }
                break;
            case VariableType.isString:
                foreach (string choice in settingValue_String_Choices)
                {
                    if (insertedValue == choice)
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
                        break;
                    }
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
                txt_SliderValue.text = settingValue_Number_Default.ToString();
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
}