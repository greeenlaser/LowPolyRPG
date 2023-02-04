using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [Header("Assignables")]
    public float walkSpeed;
    public float sprintSpeed;
    [SerializeField] private GameObject par_Managers;

    //public but hidden variables
    [HideInInspector] public bool canMove;
    [HideInInspector] public bool canSprint;
    [HideInInspector] public Vector3 velocity;

    //private variables
    private bool isSprinting;
    private float currentSpeed;
    private CharacterController theCharacterController;

    //scripts
    private Player_Camera PlayerCameraScript;
    private UI_PauseMenu PauseMenuScript;

    private void Start()
    {
        PlayerCameraScript = GetComponentInChildren<Camera>().GetComponent<Player_Camera>();
        PauseMenuScript = par_Managers.GetComponent<UI_PauseMenu>();

        theCharacterController = GetComponent<CharacterController>();
    }

    //enables player movement
    public void LoadPlayer()
    {
        currentSpeed = walkSpeed;
        canMove = true;
        canSprint = true;
        PlayerCameraScript.isCamEnabled = true;
    }
    //disables player movement
    public void UnloadPlayer()
    {
        currentSpeed = 0;
        canMove = false;
        canSprint = false;
        PlayerCameraScript.isCamEnabled = false;
    }

    private void Update()
    {
        if (canMove
            && !PauseMenuScript.isPaused)
        {
            PlayerRegularMovement();
        }
    }

    private void PlayerRegularMovement()
    {
        //movement input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        move = Vector3.ClampMagnitude(move, 1);

        //first movement update based on speed and input
        theCharacterController.Move(currentSpeed * Time.deltaTime * move);

        //final movement update based on velocity
        theCharacterController.Move(velocity * Time.deltaTime);

        //get all velocity of the controller
        Vector3 horizontalVelocity = transform.right * x + transform.forward * z;

        //enable/disable sprinting
        isSprinting = Input.GetKey(KeyCode.LeftShift);
        if (isSprinting
            && horizontalVelocity.magnitude > 0.3f)
        {
            currentSpeed = sprintSpeed;
        }
        //force-disables sprinting if the player is no longer moving but still holding down sprint key
        else if (isSprinting
                 && horizontalVelocity.magnitude < 0.3f)
        {
            isSprinting = false;
        }
        else if (!isSprinting)
        {
            currentSpeed = walkSpeed;
        }
    }
}