using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitMouse : MonoBehaviour
{
    private Vector3 onDragBeginMousePox;
    private Vector3 currentEulerRotation;
    public GameObject sculpture;
    public float pitch;
    public float yaw;
    public float roll;
    public float distToSculpture = 50f;
    public float rotationSpeed = 5.0f;
    public float sensitivity = 0.01f;
    public bool inversed = false;

    public void Start()
    {
        currentEulerRotation = new Vector3(18.49896f, 16.52547f, -22.32906f); // Initial rotation settings
    }

    public void RecordOriginalPos()
    {
        onDragBeginMousePox = Input.mousePosition;
    }

    public void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RecordOriginalPos();
        }
        Debug.Log(onDragBeginMousePox);
        if(Input.GetMouseButton(0))
        {
            // Calculate dir vector
            Vector3 moveDir = Input.mousePosition - onDragBeginMousePox;
            moveDir *= sensitivity; // Sensitivity
            moveDir.Normalize();
            // Set pitch/roll/yaw values
            pitch = moveDir.y;
            yaw = moveDir.x;
            roll = moveDir.z;
            // Clamp
            if (pitch > 68f) pitch = 89f;
            if (pitch < -89f) pitch = -89f;
            while (yaw < -180f) yaw += 360f;
            while (yaw > 180f) yaw -= 360f;

            Vector3 result = new Vector3(Mathf.Cos(yaw) * Mathf.Cos(pitch), Mathf.Sin(pitch), Mathf.Sin(yaw) * Mathf.Cos(pitch));
            Camera.main.transform.position = sculpture.transform.position - result * distToSculpture;
            Camera.main.transform.LookAt(sculpture.transform);

            // TODO change to arcball
        }
    }
}
