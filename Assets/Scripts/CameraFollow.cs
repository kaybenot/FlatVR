using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float Divider = 50f;
    public float FocusDistance = 20f;

    private Vector3 offset = Vector3.zero;
    private Vector3 follow = Vector3.zero;
    private Vector3 eulerRotation = Vector3.zero;
    private Vector3 forward = Vector3.zero;

    private void Awake()
    {
        offset = transform.position;
        forward = transform.forward;

        UDPReceiver.OnDataReceived += (opentrackData) =>
        {
            follow.x = (float)-opentrackData.x / Divider;
            follow.y = (float)opentrackData.y / Divider;
            follow.z = (float)-opentrackData.z / 50f;
            
            eulerRotation.x = (float)-opentrackData.pitch;
            eulerRotation.y = (float)opentrackData.yaw;
            eulerRotation.z = (float)opentrackData.roll;
        };
    }

    private void Update()
    {
        transform.position = offset + follow;
        transform.LookAt(offset + forward * FocusDistance);
    }
}
