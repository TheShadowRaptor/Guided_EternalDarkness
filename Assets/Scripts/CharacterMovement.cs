using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController cController;

    [Header("Varibles")]
    public float jumpHeight = 3f;
    public float speed = 6f;
    public float sprintSpeed = 9f;

    [Header("GroundCheck")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistence;
    [SerializeField] private LayerMask groundMask;

    [Header("WallCheck")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallDistence;

    [Header("Scripts")]
    [SerializeField] private PlayerLook playerLook;


    private Vector3 velocity;
    private Vector3 move;
    private float gravity = -9.81f;
    private float _speed;
    private bool onLadder;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        PlayerGravity();
        PlayerSprint();
        PlayerJump();
        PlayerGrounded();
        PlayerOnLadder();
    }

    private void PlayerMove()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (onLadder)
        {
            move = transform.right * x + transform.up * z;
            _speed = speed / 2;
        }
        else
        {
            move = transform.right * x + transform.forward * z;
        }

        cController.Move(move * _speed * Time.deltaTime);
    }

    private void PlayerGravity()
    {      
        if (!onLadder)
        {
            velocity.y += gravity * Time.deltaTime;
            cController.Move(velocity * Time.deltaTime);
        }
    }

    private bool IsGrounded()
    {
        if(Physics.CheckSphere(groundCheck.position, groundDistence, groundMask)) return true;
        return false;
    }

    private void PlayerGrounded()
    {
        //Stops raising y velocity when on ground
        if(IsGrounded() && velocity.y < 0) velocity.y = -2f;
    }

    private void PlayerJump()
    {
        if(Input.GetButtonDown("Jump") && IsGrounded() && !onLadder) velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        // WallJump
        if (IsTouchingWall() && Input.GetButtonDown("Jump") && !IsGrounded() && !onLadder)
        {
            playerLook.wallJump = true;
            velocity.y = Mathf.Sqrt(jumpHeight * -1f * gravity);
        }
    }

    private void PlayerSprint()
    {
        if(Input.GetButton("Sprint") && IsGrounded()) _speed = sprintSpeed;
        else if(IsGrounded() && (!Input.GetButton("Sprint"))) _speed = speed;
    }

    private bool IsTouchingWall()
    {
        RaycastHit hit;
        if(Physics.Raycast(wallCheck.transform.position, wallCheck.transform.TransformDirection(Vector3.forward), out hit, wallDistence, groundMask)) return true;
        return false;
    }

    private void PlayerOnLadder()
    {
        if(!onLadder && Input.GetKeyDown(KeyCode.E))
        {
            playerLook.touchingLadder = true;
            onLadder = true;
        }

        else if(onLadder && Input.GetKeyDown(KeyCode.E))
        {
            playerLook.touchingLadder = false;
            onLadder = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Player on ladder
        if (other.CompareTag("Ladder"))
        {
            PlayerOnLadder();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            onLadder = false;
        }
    }
}
