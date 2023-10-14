using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDragging : MonoBehaviour
{
    private Rigidbody rb;
    private Transform GrabPointTransform;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Grab(Transform HoldPoint)
    {
        rb.useGravity = false;
        GrabPointTransform = HoldPoint;
    }
    public void Drop()
    {
        GrabPointTransform = null;
        rb.useGravity = true;
    }

    private void FixedUpdate()
    {
        if (GrabPointTransform != null)
        {
            Vector3 ArahPindah= Vector3.Lerp(transform.position,GrabPointTransform.position, Time.deltaTime * 10f);
            rb.MovePosition(ArahPindah);
            rb.MoveRotation(GrabPointTransform.rotation);
        }
    }
}
