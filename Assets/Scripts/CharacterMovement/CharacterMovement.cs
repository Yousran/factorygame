using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{   public float moveSpeed = 5.0f;
    public float runSpeed = 10.0f;
    public float jumpForce = 10.0f;
    private bool isRunning = false;
    public float SkalaGravitasi = 5;
    public LayerMask mask;
    public float SphereRadius;
    RaycastHit hit;

    private Rigidbody rb;

    private HashSet<GameObject> objectsInSphere = new HashSet<GameObject>();
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

        rb.AddForce(Physics.gravity * (SkalaGravitasi - 1) * rb.mass);

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


        Collider[] colliders = Physics.OverlapSphere(transform.position, SphereRadius, mask);

        // Create a HashSet of GameObjects in the current frame
        HashSet<GameObject> currentFrameObjects = new HashSet<GameObject>();

        // Loop through the colliders to find the game objects
        foreach (Collider collider in colliders)
        {
            GameObject hitObject = collider.gameObject;

            // Now, you can work with the hitObject
            Debug.Log("Found object: " + hitObject.name);

            // Enable MeshRenderer
            MeshRenderer meshRenderer = hitObject.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = true;
            }

            // Add the GameObject to the HashSet for the current frame
            currentFrameObjects.Add(hitObject);
        }

        // Loop through the objects that were in the previous frame but not in the current frame
        foreach (GameObject previousFrameObject in objectsInSphere)
        {
            if (!currentFrameObjects.Contains(previousFrameObject))
            {
                // Disable MeshRenderer for objects that are no longer in the sphere
                MeshRenderer meshRenderer = previousFrameObject.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.enabled = false;
                }
            }
        }

        // Update the HashSet of objects for the current frame
        objectsInSphere = currentFrameObjects;

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, SphereRadius);
    }
}
