using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMovement : MonoBehaviour
{
    public float sensitivity = 2.0f;
    private float rotationX = 0.0f;
    public float MaxRayDistance = 3f;
    public Camera cam;

    public static int MapSizeX { get; set; } = MapGen.SizeX;
    public static int MapSizeZ { get; set; } = MapGen.SizeZ;

    public ObjectDragging ObjDrag;
    public TreeChopProgress UITree;

    public MapGen Map;
    public Transform HoldingPoint;
    private void Start()
    {
        cam = this.GetComponent<Camera>();
        if (UITree == null)
        {
            UITree = FindObjectOfType<TreeChopProgress>();
        }
        if (Map == null)
        {
            Map = FindObjectOfType<MapGen>();
        }
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
        // Rotasi Character Berdasarkan input mouse
        transform.parent.Rotate(Vector3.up * mouseX * sensitivity);

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, MaxRayDistance))
        { 
            DragObject(hit);

            PlaceAndDestroyTerrain(hit);

            ChopTree(hit);

        }
        else
        {
            UITree.TreeHealthBar.visible = false;
        }
    }
    void PlaceAndDestroyTerrain(RaycastHit _hit)
    {
        if (_hit.transform.tag == "Terrain")
        {
            if (Input.GetMouseButtonDown(1))
            {
                Map.GetChunkFromV3(new Vector3(_hit.transform.position.x + MapSizeX/Map.ChunkSize, _hit.transform.position.y, _hit.transform.position.z)).PlaceTerrain(_hit.point);
                Map.GetChunkFromV3(new Vector3(_hit.transform.position.x - MapSizeX / Map.ChunkSize, _hit.transform.position.y, _hit.transform.position.z)).PlaceTerrain(_hit.point);
                Map.GetChunkFromV3(_hit.transform.position).PlaceTerrain(_hit.point);
                Map.GetChunkFromV3(new Vector3(_hit.transform.position.x, _hit.transform.position.y, _hit.transform.position.z + MapSizeZ / Map.ChunkSize)).PlaceTerrain(_hit.point);
                Map.GetChunkFromV3(new Vector3(_hit.transform.position.x, _hit.transform.position.y, _hit.transform.position.z - MapSizeZ / Map.ChunkSize)).PlaceTerrain(_hit.point);
                Map.GetChunkFromV3(new Vector3(_hit.transform.position.x + MapSizeX / Map.ChunkSize, _hit.transform.position.y, _hit.transform.position.z + MapSizeZ / Map.ChunkSize)).PlaceTerrain(_hit.point);
                Map.GetChunkFromV3(new Vector3(_hit.transform.position.x - MapSizeX / Map.ChunkSize, _hit.transform.position.y, _hit.transform.position.z - MapSizeZ / Map.ChunkSize)).PlaceTerrain(_hit.point);
                Map.GetChunkFromV3(new Vector3(_hit.transform.position.x + MapSizeX / Map.ChunkSize, _hit.transform.position.y, _hit.transform.position.z - MapSizeZ / Map.ChunkSize)).PlaceTerrain(_hit.point);
                Map.GetChunkFromV3(new Vector3(_hit.transform.position.x - MapSizeX / Map.ChunkSize, _hit.transform.position.y, _hit.transform.position.z + MapSizeZ / Map.ChunkSize)).PlaceTerrain(_hit.point);
            }
            if (Input.GetMouseButtonDown(0))
            {
                Map.GetChunkFromV3(new Vector3(_hit.transform.position.x + MapSizeX / Map.ChunkSize, _hit.transform.position.y, _hit.transform.position.z)).RemoveTerrain(_hit.point);
                Map.GetChunkFromV3(new Vector3(_hit.transform.position.x - MapSizeX / Map.ChunkSize, _hit.transform.position.y, _hit.transform.position.z)).RemoveTerrain(_hit.point);
                Map.GetChunkFromV3(_hit.transform.position).RemoveTerrain(_hit.point);
                Map.GetChunkFromV3(new Vector3(_hit.transform.position.x, _hit.transform.position.y, _hit.transform.position.z + MapSizeZ / Map.ChunkSize)).RemoveTerrain(_hit.point);
                Map.GetChunkFromV3(new Vector3(_hit.transform.position.x, _hit.transform.position.y, _hit.transform.position.z - MapSizeZ / Map.ChunkSize)).RemoveTerrain(_hit.point);
                Map.GetChunkFromV3(new Vector3(_hit.transform.position.x + MapSizeX / Map.ChunkSize, _hit.transform.position.y, _hit.transform.position.z + MapSizeZ / Map.ChunkSize)).RemoveTerrain(_hit.point);
                Map.GetChunkFromV3(new Vector3(_hit.transform.position.x - MapSizeX / Map.ChunkSize, _hit.transform.position.y, _hit.transform.position.z - MapSizeZ / Map.ChunkSize)).RemoveTerrain(_hit.point);
                Map.GetChunkFromV3(new Vector3(_hit.transform.position.x + MapSizeX / Map.ChunkSize, _hit.transform.position.y, _hit.transform.position.z - MapSizeZ / Map.ChunkSize)).RemoveTerrain(_hit.point);
                Map.GetChunkFromV3(new Vector3(_hit.transform.position.x - MapSizeX / Map.ChunkSize, _hit.transform.position.y, _hit.transform.position.z + MapSizeZ / Map.ChunkSize)).RemoveTerrain(_hit.point);
            }
        }
    }
    void DragObject(RaycastHit _hit)
    {
        if (ObjDrag == null && Input.GetMouseButton(0))
        {
            if (_hit.transform.TryGetComponent(out ObjDrag))
            {
                ObjDrag.Grab(HoldingPoint);
            }
        }
        else if (ObjDrag != null && !Input.GetMouseButton(0))
        {
            ObjDrag.Drop();
            ObjDrag = null;
        }
    }
    void ChopTree(RaycastHit _hit)
    {
        if (_hit.transform.GetComponentInParent<TreeStats>())
        {
            if (Input.GetMouseButtonDown(0))
            {
                _hit.transform.GetComponentInParent<TreeStats>().HealthPohon -= 1;
            }
            UITree.TreeHealthBar.value = _hit.transform.GetComponentInParent<TreeStats>().HealthPohon / _hit.transform.GetComponentInParent<TreeStats>().MaxHealthPohon * 100;
            UITree.TreeHealthBar.visible = true;
            if (_hit.transform.GetComponentInParent<TreeStats>().HealthPohon <= 0)
            {
                _hit.transform.GetComponentInParent<TreeStats>().TreeIsChopped();
                UITree.TreeHealthBar.visible = false;
            }
        }
        else
        {
            UITree.TreeHealthBar.visible = false;
        }
    }
}
