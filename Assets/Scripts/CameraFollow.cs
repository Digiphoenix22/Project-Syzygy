using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The target the camera should follow
    public float smoothSpeed = 0.125f; // Adjust this value to change how smoothly the camera follows the target
    public Vector3 offset; // Offset from the target position (you can adjust this to set how far back the camera should be from the target)

    void FixedUpdate()//THIS WAS FIXED UPDATE BUT I CHANGED IT BECAUSE MOVEMENT WAS JITTERY. IF SOMETHING BROKE THAT IS WHY
    {
        // Check if target is assigned to avoid NullReferenceException
        if (target == null) return;

        // Desired position is the target position plus the offset
        Vector3 desiredPosition = target.position + offset;
        // Interpolated position is a mix of the current position and the desired position, for smooth movement
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        // Apply the interpolated position to the camera
        transform.position = smoothedPosition;

        // Optional: Make the camera always look at the target
        //transform.LookAt(target);
    }
}