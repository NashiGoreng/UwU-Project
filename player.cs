using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FPSControllerRB : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    private Rigidbody rb;
    private bool isGrounded;
    private float rotationX = 0;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Mencegah Rigidbody berputar karena fisika
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    [System.Obsolete]
    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    [System.Obsolete]
    void HandleMovement()
    {
        // Mendapatkan input gerakan
        float moveX = Input.GetAxis("Horizontal"); // A/D
        float moveZ = Input.GetAxis("Vertical");   // W/S
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        float speed = isRunning ? runSpeed : walkSpeed;
        Vector3 moveDirection = transform.TransformDirection(new Vector3(moveX, 0, moveZ) * speed);

        // Tetapkan velocity pada Rigidbody
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);

        // Melompat jika menekan tombol Jump dan sedang di tanah
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    void HandleRotation()
    {
        // Rotasi kamera ke atas dan ke bawah
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        // Rotasi karakter ke kiri dan kanan
        float rotationY = Input.GetAxis("Mouse X") * lookSpeed;
        transform.Rotate(0, rotationY, 0);
    }

    void OnCollisionStay(Collision collision)
    {
        // Mengecek apakah karakter menyentuh tanah
        isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        // Jika tidak menyentuh tanah, isGrounded = false
        isGrounded = false;
    }
}