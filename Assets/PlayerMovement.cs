using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // ================= Varibles ==================
    [Header("Components")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform groundCheck;

    [Header("LayerMasks")]
    [SerializeField] private LayerMask groundMask;

    [Header("RayValues")]
    [SerializeField] private float groundDistance = 0.4f;

    [Header("MovementValues")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float jogSpeed = 7f;
    [SerializeField] private float sprintSpeed = 10f;

    // ================== private ==================

    private Vector3 velocity;

    //Acceleration Varibles
    private float accelerationSpeed = 3;
    private float maxAcceleration = 0;
    private float minAcceleration = 0;

    // ================== public ===================
    [HideInInspector] public float acceleration;
    
    // =============================================

    // Start is called before the first frame update
    void Start()
    {
        maxAcceleration = jogSpeed;
        minAcceleration = jogSpeed / 2;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    // =========================== Moveing Methods =====================
    private void MovePlayer()
    {

        // Setting up controls
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = (transform.right * x) + (transform.forward * z);

        // Moves Player
        controller.Move(move * acceleration * Time.deltaTime);

        // Gravity
        PlayerGravity();

        // JumpControls
        PlayerJump();

        // Building Speed
        ManageAcceleration();
    }

    private bool IsSprinting()
    {
        if (Input.GetButton("Sprint"))
        {
            return true;
        }
        return false;
    }

    private void PlayerGravity()
    {
        // Stops raising gravity when player is on ground
        if (IsGrounded() && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Adds gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private bool IsGrounded()
    {
        // Checks if player is grounded
        if (Physics.CheckSphere(groundCheck.position, groundDistance, groundMask))
        {
            return true;
        }
        return false;
    }

    private void PlayerJump()
    {
        if(Input.GetButtonDown("Jump") && IsGrounded())
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
    }

    private bool IsMoving()
    {
        if (Input.GetAxis("Horizontal") == 1) return true;
        if (Input.GetAxis("Horizontal") == -1) return true;
        if (Input.GetAxis("Vertical") == 1) return true;
        if (Input.GetAxis("Vertical") == -1) return true;
        return false;
    }

    private void ManageAcceleration()
    {
        if (IsSprinting())
        {
            maxAcceleration = sprintSpeed;
        }
        else
        {
            maxAcceleration = jogSpeed;
        }

        if (IsMoving())
        {
            acceleration += accelerationSpeed * Time.deltaTime * 2;
            if (acceleration >= maxAcceleration)
            {
                acceleration = maxAcceleration;
            }
        }
        else
        {
            acceleration -= accelerationSpeed * Time.deltaTime * 2;
            if (acceleration <= minAcceleration)
            {
                acceleration = minAcceleration;
            }
        }
    }

    // ===============================================================
}
