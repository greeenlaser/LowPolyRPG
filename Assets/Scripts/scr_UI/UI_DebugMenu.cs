using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_DebugMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text txt_FPS;

    //private variables
    private float timer;
    private float deltaTime;

    private void Start()
    {
        Application.targetFrameRate = 999;
    }

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float msec = Mathf.FloorToInt(deltaTime * 1000.0f);
        float fps = Mathf.FloorToInt(1.0f / deltaTime);

        timer += Time.unscaledDeltaTime;
        if (timer > 0.1f)
        {
            txt_FPS.text = fps + " (" + msec + ")";
            timer = 0;
        }
    }
}