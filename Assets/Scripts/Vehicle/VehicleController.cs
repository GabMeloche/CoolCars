﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class VehicleController : MonoBehaviour
{
    public bool m_automaticGearbox = true;
    public float m_forwardTorque = 1000f;
    public float m_reverseTorque = 500f;
    public float m_steeringSpeed = 0.05f;
    public float m_maxSteerAngle = 60f;
    public float m_totalGears = 5;
    public float m_speed { get; set; } = 0;
    public int m_currentGear { get; set; } = 1;
    public int m_currentRPM { get; set; } = 1500;

    private bool m_reversing = false;
    private int m_maxRPM = 8500;
    private float[] m_maxSpeeds = new float[]
    {
        5f,
        10f,
        15f,
        20f,
        25f
    };

    private Rigidbody m_rigidbody;
    private List<WheelCollider> m_motorWheels = new List<WheelCollider>();
    private List<WheelCollider> m_steeringWheels = new List<WheelCollider>();

    void Start()
    {
        Wheel[] AllWheels;
        AllWheels = GetComponentsInChildren<Wheel>();

        m_rigidbody = GetComponent<Rigidbody>();

        foreach (Wheel wheel in AllWheels)
        {
            if (wheel.m_isMotor)
                m_motorWheels.Add(wheel.m_wheel);

            if (wheel.m_IsSteering)
                m_steeringWheels.Add(wheel.m_wheel);
        }
    }

    void Update()
    {
        m_currentRPM = (int)(m_maxRPM * m_speed / m_maxSpeeds[m_currentGear - 1]);
        m_currentRPM = Mathf.Clamp(m_currentRPM, 1500, m_maxRPM);
        m_speed = transform.InverseTransformDirection(m_rigidbody.velocity).z;

        if (m_speed > m_maxSpeeds[m_currentGear - 1] && m_currentGear < m_totalGears)
            ++m_currentGear;
        else if (m_currentGear > 1 && m_speed < m_maxSpeeds[m_currentGear - 2])
            --m_currentGear;
    }

    void FixedUpdate()
    {
        Accelerate(Input.GetKey(KeyCode.W));
        Steer(Input.GetAxis("Horizontal"));
    }

    private void Accelerate(bool p_shouldAccelerate)
    {
        if (p_shouldAccelerate)
        {
            foreach (WheelCollider wheel in m_motorWheels)
                wheel.motorTorque = m_currentRPM / 4;
        }
        else
        {
            foreach (WheelCollider wheel in m_motorWheels)
                wheel.motorTorque = 0f;
        }
    }

    private void Steer(float p_direction)
    {
        foreach (WheelCollider wheel in m_steeringWheels)
        {
            if (Mathf.Approximately(p_direction, 0f))
                wheel.steerAngle = Mathf.Lerp(0f, wheel.steerAngle, Time.deltaTime * m_steeringSpeed);
            else if(p_direction < 0)
                wheel.steerAngle = Mathf.Lerp(-m_maxSteerAngle, 0f, Time.deltaTime * m_steeringSpeed);
            else
                wheel.steerAngle = Mathf.Lerp(m_maxSteerAngle, 0f, Time.deltaTime * m_steeringSpeed);          
        }

    }

    private void Reverse(bool p_shouldReverse)
    {
        if (!p_shouldReverse)
            return;

        if (Mathf.Approximately(m_rigidbody.velocity.magnitude, 0f))
        {

        }
    }

    private void SwitchGears()
    {

    }
    /*private void BrakeOrReverse(bool p_keyPressed)
    {
        if (!p_keyPressed)
            return;

        if (Mathf.Approximately(m_rigidbody.velocity.magnitude, 0f))
        {
            m_reversing = !m_reversing;
        }
    }*/
}
