using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{   public float moveSpeed = 5.0f;
    public float runSpeed = 10.0f;
    public float jumpForce = 10.0f;
    private bool isRunning = false;
    RaycastHit hit;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Mengunci kursor mouse ke tengah layar dan menyembunyikannya
        Cursor.lockState = CursorLockMode.Locked;
    }
    void FixedUpdate(){
        // Input pengguna untuk mengontrol karakter
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float mouseX = Input.GetAxis("Mouse X");

        // Mengatur rotasi karakter berdasarkan input mouse X
        transform.Rotate(Vector3.up * mouseX);
        // Memeriksa apakah karakter berlari
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        // Menggerakkan karakter berdasarkan speed
        Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        moveDirection.Normalize();
        float speed = isRunning ? runSpeed : moveSpeed;
        Vector3 newPosition = transform.position + moveDirection * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    void Update()
    {
        // Lompat
        if (Input.GetKey(KeyCode.Space) && Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f))
        {
            rb.AddForce(Vector3.up * jumpForce/10, ForceMode.Impulse);
        }
        
    }

}
