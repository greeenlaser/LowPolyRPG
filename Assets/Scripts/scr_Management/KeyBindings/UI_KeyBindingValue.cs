using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_KeyBindingValue : MonoBehaviour
{
    public string keyBindName;
    public string keyBindValue_Default;
    [HideInInspector] public string keyBindValue;
    [HideInInspector] public Button btn_KeyBind;
    [HideInInspector] public TMP_Text txt_ButtonText;

    //scripts
    private Manager_KeyBindings KeyBindsScript;
    private UI_PauseMenu PauseMenuScript;

    private void Awake()
    {
        KeyBindsScript = FindObjectOfType<Manager_KeyBindings>();
        PauseMenuScript = FindObjectOfType<UI_PauseMenu>();

        btn_KeyBind = GetComponentInChildren<Button>();
        txt_ButtonText = btn_KeyBind.transform.GetComponentInChildren<TMP_Text>();

        btn_KeyBind.onClick.AddListener(ChangeKey);
    }

    //switch the key of this button to new desired value
    public void ChangeKey()
    {
        if (!PauseMenuScript.isChangingKey)
        {
            KeyBindsScript.StartKeyAssign(keyBindName);
        }
    }
}