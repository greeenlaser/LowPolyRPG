using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Camera : MonoBehaviour
{
    //public but hidden variables
    [HideInInspector] public bool isCamEnabled;
    [HideInInspector] public float sensX;
    [HideInInspector] public float sensY;

    //private variables
    private float mouseX;
    private float mouseY;
    private float xRot;

    private void Start()
    {
        StartCoroutine(Wait());
    }

    private void Update()
    {
        if (isCamEnabled)
        {
            mouseX = Input.GetAxis("Mouse X") * sensX * 6 * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * sensY * 6 * Time.deltaTime;

            xRot -= mouseY;

            xRot = Mathf.Clamp(xRot, -90f, 90f);
            transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);

            transform.parent.Rotate(Vector3.up * mouseX);
        }
    }

    //waits 0.2 seconds before activating the players camera
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.2f);

        sensX = 50;
        sensY = 50;
    }
}