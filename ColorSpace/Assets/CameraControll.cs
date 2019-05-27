using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    private const float standartMinZ = 0.1f;

    private Vector3 targetLocalPosition;
    public float minZ = 0.1f;
    public float scrollPower = 2f;

    public Transform transform_rotationX;
    public Transform transform_rotationY;
    private Vector3 mousePosition;
    Vector3 deltaMousePosition;

    void Start()
    {
        targetLocalPosition = transform.localPosition;
        mousePosition = Input.mousePosition;
        deltaMousePosition = Vector3.zero;
    }

    void Update()
    {
        {
            if (minZ < 0)
            {
                Debug.LogWarning("Little accident occurred and value was out of bounds");
                minZ = standartMinZ;
            }
            float scrollMovement = Input.GetAxis("Mouse ScrollWheel") * scrollPower;
            targetLocalPosition += Vector3.forward * scrollMovement;
            if (targetLocalPosition.z < minZ)
            {
                targetLocalPosition += Vector3.forward * minZ;
            }
            this.transform.localPosition = Vector3.Lerp(transform.localPosition, targetLocalPosition, 0.125f);
        }
        if (Input.GetMouseButton(0))
        {
            deltaMousePosition = Input.mousePosition - mousePosition;
        }
        if (Input.GetMouseButton(2))
        {
            deltaMousePosition = Vector3.zero;
        }
        if (deltaMousePosition != Vector3.zero)
        {
            transform_rotationX.Rotate(Vector3.up, deltaMousePosition.x);
            transform_rotationY.Rotate(Vector3.right, deltaMousePosition.y);
        }
        mousePosition = Input.mousePosition;
    }
}
