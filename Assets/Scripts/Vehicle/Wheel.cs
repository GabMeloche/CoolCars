using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    private WheelCollider m_wheel;

    void Start()
    {
        m_wheel = GetComponentInParent<WheelCollider>();
    }

    void Update()
    {
        RotateWheelVisually();

        Vector3 position;
        Quaternion rotation;
        m_wheel.GetWorldPose(out position, out rotation);

        Debug.Log(gameObject.name + ": " + (rotation == transform.rotation));
    }

    public void RotateWheelVisually()
    {
        Vector3 position;
        Quaternion rotation;
        m_wheel.GetWorldPose(out position, out rotation);
        //transform.position = position;
        transform.rotation = rotation;
    }
}
