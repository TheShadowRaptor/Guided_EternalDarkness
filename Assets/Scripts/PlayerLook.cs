using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("Varibles")]
    [SerializeField] private float rotationSpeed = 30f;
    public float mouseSensitivity = 100f;
    public bool wallJump = false;

    [Header("Transforms")]
    [SerializeField] private Transform player;
    
    private float xRotation = 0f;
    private Quaternion rotDest;

    // Start is called before the first frame update
    void Start()
    {
        //Lock Cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Created ints for mouse movement
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;    
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //Players ability to look with mouse movement
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);

        Turn180();
    }

    private void Turn180()
    {
        if(wallJump)
        {
            player.Rotate(Vector3.up, 180);
            wallJump = false;
        }
        
    }
}
