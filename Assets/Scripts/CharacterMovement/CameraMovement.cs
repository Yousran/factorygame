using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float sensitivity = 2.0f;
    private float rotationX = 0.0f;

    void Update()
    {
        // Input pengguna untuk mengontrol rotasi kamera
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Menghitung rotasi vertikal (dalam batas tertentu)
        rotationX -= mouseY * sensitivity;
        rotationX = Mathf.Clamp(rotationX, -90.0f, 90.0f);

        // Rotasi kamera berdasarkan input mouse X dan Y
        transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.parent.Rotate(Vector3.up * mouseX * sensitivity);
    }
}
