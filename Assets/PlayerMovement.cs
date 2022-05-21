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
    [SerializeField] private float speed = 12f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3f;

    // ================== private ==================

    private Vector3 velocity;

    // =============================================

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        // Setting up controls
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = (transform.right * x) + (transform.forward * z);

        // Moves Player
        controller.Move(move * speed * Time.deltaTime);

        // Gravity
        PlayerGravity();

        // JumpControls
        PlayerJump();
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
}
