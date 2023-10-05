using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float sensitivity = 2.0f;
    private float rotationX = 0.0f;
    public Camera cam;

    public MapGen Map;
    private void Start()
    {
        cam = this.GetComponent<Camera>();
    }

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


        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.transform.tag == "Terrain")
                {
                    Map.GetChunkFromV3(hit.transform.position).PlaceTerrain(hit.point);
                    Map.GetChunkFromV3(new Vector3(hit.transform.position.x + 20, hit.transform.position.y, hit.transform.position.z)).PlaceTerrain(hit.point);
                    Map.GetChunkFromV3(new Vector3(hit.transform.position.x - 20, hit.transform.position.y, hit.transform.position.z)).PlaceTerrain(hit.point);
                    Map.GetChunkFromV3(new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z + 20)).PlaceTerrain(hit.point);
                    Map.GetChunkFromV3(new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z - 20)).PlaceTerrain(hit.point);
                    Map.GetChunkFromV3(new Vector3(hit.transform.position.x + 20, hit.transform.position.y, hit.transform.position.z + 20)).PlaceTerrain(hit.point);
                    Map.GetChunkFromV3(new Vector3(hit.transform.position.x - 20, hit.transform.position.y, hit.transform.position.z - 20)).PlaceTerrain(hit.point);
                    Map.GetChunkFromV3(new Vector3(hit.transform.position.x + 20, hit.transform.position.y, hit.transform.position.z - 20)).PlaceTerrain(hit.point);
                    Map.GetChunkFromV3(new Vector3(hit.transform.position.x - 20, hit.transform.position.y, hit.transform.position.z + 20)).PlaceTerrain(hit.point);
                }
            }
        }
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Terrain")
                {
                    Map.GetChunkFromV3(hit.transform.position).RemoveTerrain(hit.point);
                    Map.GetChunkFromV3(new Vector3(hit.transform.position.x+20, hit.transform.position.y, hit.transform.position.z)).RemoveTerrain(hit.point);
                    Map.GetChunkFromV3(new Vector3(hit.transform.position.x - 20, hit.transform.position.y, hit.transform.position.z)).RemoveTerrain(hit.point);
                    Map.GetChunkFromV3(new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z + 20)).RemoveTerrain(hit.point);
                    Map.GetChunkFromV3(new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z - 20)).RemoveTerrain(hit.point);
                    Map.GetChunkFromV3(new Vector3(hit.transform.position.x + 20, hit.transform.position.y, hit.transform.position.z + 20)).RemoveTerrain(hit.point);
                    Map.GetChunkFromV3(new Vector3(hit.transform.position.x - 20, hit.transform.position.y, hit.transform.position.z - 20)).RemoveTerrain(hit.point);
                    Map.GetChunkFromV3(new Vector3(hit.transform.position.x + 20, hit.transform.position.y, hit.transform.position.z - 20)).RemoveTerrain(hit.point);
                    Map.GetChunkFromV3(new Vector3(hit.transform.position.x - 20, hit.transform.position.y, hit.transform.position.z + 20)).RemoveTerrain(hit.point);
                }
            }
        }
    }
}
