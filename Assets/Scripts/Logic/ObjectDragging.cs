using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDragging : MonoBehaviour
{
    private Vector3 OffsetMouse;
    private float mousezcoord;
    private void OnMouseDown()
    {
        OffsetMouse = gameObject.transform.position - GetMouseWorldPos();
    }
    Vector3 GetMouseWorldPos()
    {
        Vector3 point = Input.mousePosition;

        point.z = mousezcoord;

        return Camera.main.ScreenToWorldPoint(point);


    }
    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + OffsetMouse;
    }
}
