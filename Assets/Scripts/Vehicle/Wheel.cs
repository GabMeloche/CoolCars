using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public bool m_isMotor;
    public bool m_IsSteering;

    public WheelCollider m_wheel;
    /*{
        get { return m_wheel; }
        private set { m_wheel = value; }
    }*/

    void Awake()
    {
        m_wheel = GetComponentInParent<WheelCollider>();
    }

    void FixedUpdate()
    {
        RotateWheelVisually();
    }

    public void RotateWheelVisually()
    {
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;
        m_wheel.GetWorldPose(out pos, out rot);
        rot = rot * Quaternion.Euler(new Vector3(0, 0, 90));
        transform.position = pos;
        transform.rotation = rot;
    }

    public void Drift()
    {

    }
}
