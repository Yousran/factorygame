using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float sensitivity = 2.0f;
    private float rotationX = 0.0f;
    public Camera cam;

    public static int MapSizeX { get; set; } = MapGen.SizeX;
    public static int MapSizeZ { get; set; } = MapGen.SizeZ;


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
                //if (hit.transform.tag == "Terrain")
                //{
                //    Map.GetChunkFromV3(hit.transform.position).PlaceTerrain(hit.point);
                //    Map.GetChunkFromV3(new Vector3(hit.transform.position.x + MapSizeX/Map.ChunkSize, hit.transform.position.y, hit.transform.position.z)).PlaceTerrain(hit.point);
                //    Map.GetChunkFromV3(new Vector3(hit.transform.position.x - MapSizeX / Map.ChunkSize, hit.transform.position.y, hit.transform.position.z)).PlaceTerrain(hit.point);
                //    Map.GetChunkFromV3(new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z + MapSizeZ / Map.ChunkSize)).PlaceTerrain(hit.point);
                //    Map.GetChunkFromV3(new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z - MapSizeZ / Map.ChunkSize)).PlaceTerrain(hit.point);
                //    Map.GetChunkFromV3(new Vector3(hit.transform.position.x + MapSizeX / Map.ChunkSize, hit.transform.position.y, hit.transform.position.z + MapSizeZ / Map.ChunkSize)).PlaceTerrain(hit.point);
                //    Map.GetChunkFromV3(new Vector3(hit.transform.position.x - MapSizeX / Map.ChunkSize, hit.transform.position.y, hit.transform.position.z - MapSizeZ / Map.ChunkSize)).PlaceTerrain(hit.point);
                //    Map.GetChunkFromV3(new Vector3(hit.transform.position.x + MapSizeX / Map.ChunkSize, hit.transform.position.y, hit.transform.position.z - MapSizeZ / Map.ChunkSize)).PlaceTerrain(hit.point);
                //    Map.GetChunkFromV3(new Vector3(hit.transform.position.x - MapSizeX / Map.ChunkSize, hit.transform.position.y, hit.transform.position.z + MapSizeZ / Map.ChunkSize)).PlaceTerrain(hit.point);
                //}
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //Chop Tree Logic
                if (hit.transform.GetComponentInParent<TreeStats>())
                {
                    hit.transform.GetComponentInParent<TreeStats>().HealthPohon -= 1;
                    Debug.Log(hit.transform.GetComponentInParent<TreeStats>().HealthPohon);
                    if (hit.transform.GetComponentInParent<TreeStats>().HealthPohon <= 0)
                    {
                        hit.transform.GetComponentInParent<TreeStats>().TreeIsChopped();
                    }
                }
                //if (hit.transform.tag == "Terrain")
                //{
                //    Map.GetChunkFromV3(hit.transform.position).RemoveTerrain(hit.point);
                //    Map.GetChunkFromV3(new Vector3(hit.transform.position.x+ MapSizeX / Map.ChunkSize, hit.transform.position.y, hit.transform.position.z)).RemoveTerrain(hit.point);
                //    Map.GetChunkFromV3(new Vector3(hit.transform.position.x - MapSizeX / Map.ChunkSize, hit.transform.position.y, hit.transform.position.z)).RemoveTerrain(hit.point);
                //    Map.GetChunkFromV3(new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z + MapSizeZ / Map.ChunkSize)).RemoveTerrain(hit.point);
                //    Map.GetChunkFromV3(new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z - MapSizeZ / Map.ChunkSize)).RemoveTerrain(hit.point);
                //    Map.GetChunkFromV3(new Vector3(hit.transform.position.x + MapSizeX / Map.ChunkSize, hit.transform.position.y, hit.transform.position.z + MapSizeZ / Map.ChunkSize)).RemoveTerrain(hit.point);
                //    Map.GetChunkFromV3(new Vector3(hit.transform.position.x - MapSizeX / Map.ChunkSize, hit.transform.position.y, hit.transform.position.z - MapSizeZ / Map.ChunkSize)).RemoveTerrain(hit.point);
                //    Map.GetChunkFromV3(new Vector3(hit.transform.position.x + MapSizeX / Map.ChunkSize, hit.transform.position.y, hit.transform.position.z - MapSizeZ / Map.ChunkSize)).RemoveTerrain(hit.point);
                //   Map.GetChunkFromV3(new Vector3(hit.transform.position.x - MapSizeX / Map.ChunkSize, hit.transform.position.y, hit.transform.position.z + MapSizeZ / Map.ChunkSize)).RemoveTerrain(hit.point);
                //}
            }
        }
    }
}
