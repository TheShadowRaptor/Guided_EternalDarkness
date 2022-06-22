using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    [Header("Varibles")]
    public float jumpHeight = 3f;
    public float speed = 6f;
    public float sprintSpeed = 9f;
    [Header("GroundCheck")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistence;
    [SerializeField] private LayerMask groundMask;

    public CharacterController cController;

    private Vector3 velocity;
    private float gravity = -9.81f;
    private float _speed;

    // Start is called before the first frame update
    void Start()
    {
        _speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        PlayerGravity();
        PlayerSprint();
        PlayerJump();
        PlayerGrounded();

    }

    private void PlayerMove()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        cController.Move(move * _speed * Time.deltaTime);
    }

    private void PlayerGravity()
    {
        velocity.y += gravity * Time.deltaTime;

        cController.Move(velocity * Time.deltaTime);
    }

    private bool IsGrounded()
    {
        if(Physics.CheckSphere(groundCheck.position, groundDistence, groundMask))
        {
            return true;
        }
        return false;
    }

    private void PlayerGrounded()
    {
        if(IsGrounded() && velocity.y < 0) velocity.y = -2f;
    }

    private void PlayerJump()
    {
        if(Input.GetButtonDown("Jump") && IsGrounded()) velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    private void PlayerSprint()
    {
        //sprint
        if (Input.GetButton("Sprint") && IsGrounded()) _speed = sprintSpeed;
        else if(IsGrounded()) _speed = speed;
    }
}
