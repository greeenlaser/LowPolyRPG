using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [Header("Assignables")]
    public float walkSpeed;
    public float jumpHeight;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private GameObject checkSphere;
    [SerializeField] private GameObject thePlayerCamera;
    [SerializeField] private GameObject par_Managers;

    //public but hidden variables
    [HideInInspector] public bool canMove;
    [HideInInspector] public bool canSprint;
    [HideInInspector] public float currentSpeed;
    [HideInInspector] public Vector3 velocity;

    //private variables
    private bool isGrounded;
    private bool isCrouching;
    private bool canJump;
    private bool isJumping;
    private bool isSprinting;
    private float minVelocity;
    private readonly float gravity = -9.81f;
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
        canJump = true;
        PlayerCameraScript.isCamEnabled = true;
    }
    //disables player movement
    public void UnloadPlayer()
    {
        currentSpeed = 0;
        canMove = false;
        canSprint = false;
        canJump = false;
        PlayerCameraScript.isCamEnabled = false;
    }

    private void Update()
    {
        if (canMove
            && !PauseMenuScript.isPaused)
        {
            //check if player is grounded
            if (Physics.CheckSphere(checkSphere.transform.position,
                                    0.4f,
                                    groundMask))
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }

            PlayerRegularMovement();
        }
    }

    private void PlayerRegularMovement()
    {
        //gravity if player is grounded
        if (velocity.y < 0
            && isGrounded)
        {
            //get smallest velocity
            if (velocity.y < minVelocity)
            {
                minVelocity = velocity.y;
            }

            //check if smallest velocity is less than or equal to -25f
            if (minVelocity <= -25f)
            {
                minVelocity = -2f;
            }

            velocity.y = -2f;
        }

        //gravity if player isnt grounded
        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime * 4f;
        }

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
            if (isCrouching)
            {
                theCharacterController.height = 2;
                thePlayerCamera.transform.localPosition = new(0, 0.6f, 0);
                isCrouching = false;
            }

            currentSpeed = walkSpeed * 2;
        }
        //force-disables sprinting if the player is no longer moving but still holding down sprint key
        else if (isSprinting
                 && horizontalVelocity.magnitude < 0.3f)
        {
            isSprinting = false;
        }
        else if (!isSprinting
                 && !isJumping)
        {
            currentSpeed = walkSpeed;
        }

        //enable/disable jumping
        if (Input.GetKeyDown(KeyCode.Space)
            && isGrounded
            && !isJumping
            && canJump)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -5.2f * gravity);
            theCharacterController.stepOffset = 0;

            isJumping = true;
        }
        //stop jumping
        if (isGrounded
             && isJumping)
        {
            theCharacterController.stepOffset = 0.3f;
            isJumping = false;
        }

        //enable/disable crouching
        if (Input.GetKeyDown(KeyCode.LeftControl)
            && isGrounded)
        {
            isCrouching = !isCrouching;

            if (isSprinting)
            {
                isSprinting = false;
            }

            if (isCrouching)
            {
                currentSpeed = walkSpeed / 2;

                theCharacterController.height = 1f;

                thePlayerCamera.transform.localPosition = new(0, 0.3f, 0);
            }
            else if (!isCrouching)
            {
                currentSpeed = walkSpeed;

                theCharacterController.height = 2;

                thePlayerCamera.transform.localPosition = new(0, 0.6f, 0);
            }
        }
    }
}