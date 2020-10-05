using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public bool m_automaticGearbox = true;
    public float m_forwardTorqueMultiplier = 0.25f;
    public float m_brakeTorque = 1000f;
    public float m_reverseTorque = 500f;
    public float m_steeringSpeed = 0.05f;
    public float m_maxSteerAngle = 60f;
    public float m_totalGears = 5;
    public int m_maxRPM = 8500;
    public int m_minRPM = 1500;
    public int m_currentGear { get; set; } = 1;
    public int m_currentRPM { get; set; } = 1500;
    public float m_speed { get; set; } = 0;
    public float[] m_maxSpeeds = new float[]
    {
        5f,
        10f,
        15f,
        20f,
        25f
    };

    private int m_lastGear = 1;

    private Rigidbody m_rigidbody;
    private List<WheelCollider> m_motorWheels = new List<WheelCollider>();
    private List<WheelCollider> m_steeringWheels = new List<WheelCollider>();
    private WheelCollider[] m_allWheelColliders;

    void Start()
    {
        m_allWheelColliders = GetComponentsInChildren<WheelCollider>();
        Wheel[] AllWheels = GetComponentsInChildren<Wheel>();

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
        Accelerate(Input.GetAxis("Vertical"));
        Steer(Input.GetAxis("Horizontal"));

        if (m_currentGear >= 0)
            Brake(Input.GetKey(KeyCode.S));
        else
        {
            Reverse(Input.GetAxis("Vertical"));
            Brake(Input.GetKey(KeyCode.W));
        }


        if (Input.GetAxis("Vertical") < 0 && m_speed <= 0 && Input.GetKeyDown(KeyCode.S)) 
            m_currentGear = -1;
        else if (Input.GetAxis("Vertical") > 0 && m_speed >= 0 && Input.GetKeyDown(KeyCode.W))
            m_currentGear = 1;

        if (Input.GetKeyDown(KeyCode.LeftControl))
            SetNeutral(true);
        else if (Input.GetKeyUp(KeyCode.LeftControl))
            SetNeutral(false);

        if (m_currentGear > 0)
        {
            m_currentRPM = (int)(m_maxRPM * m_speed / m_maxSpeeds[m_currentGear - 1]);
            m_currentRPM = Mathf.Clamp(m_currentRPM, m_minRPM, m_maxRPM);

            if (m_speed > m_maxSpeeds[m_currentGear - 1] && m_currentGear < m_totalGears)
                ++m_currentGear;
            else if (m_currentGear > 1 && m_speed < m_maxSpeeds[m_currentGear - 2])
                --m_currentGear;
        }
        else if (m_currentGear == 0)
            m_currentRPM = (int)Mathf.Lerp(m_minRPM, m_maxRPM, Mathf.Abs(Input.GetAxis("Vertical"))); 
    }

    void FixedUpdate()
    {
        m_speed = transform.InverseTransformDirection(m_rigidbody.velocity).z;
    }

    private void Accelerate(float p_forwardInput)
    {
        if (m_currentGear == -1)
            return;

        if (p_forwardInput >= 0f)
        {
            foreach (WheelCollider wheel in m_motorWheels)
            {
                if (m_currentGear > 0)
                    wheel.motorTorque = m_currentRPM * m_forwardTorqueMultiplier * p_forwardInput;
            }
        }
    }

    private void Steer(float p_direction)
    {
        foreach (WheelCollider wheel in m_steeringWheels)
        {
            if (Mathf.Approximately(p_direction, 0f))
                wheel.steerAngle = Mathf.Lerp(0f, wheel.steerAngle, Time.deltaTime * m_steeringSpeed);
            else if (p_direction < 0f)
                wheel.steerAngle = Mathf.Lerp(-m_maxSteerAngle, 0f, Time.deltaTime * m_steeringSpeed);
            else
                wheel.steerAngle = Mathf.Lerp(m_maxSteerAngle, 0f, Time.deltaTime * m_steeringSpeed);
        }

    }

    public void Brake(bool p_shouldBrake)
    {
        foreach (WheelCollider wheel in m_allWheelColliders)
        {
            if (p_shouldBrake)
                wheel.brakeTorque = m_brakeTorque;
            else
                wheel.brakeTorque = 0f;
        }
    }

    public void SetNeutral(bool p_setToNeutral)
    {
        if (p_setToNeutral)
        {
            m_lastGear = m_currentGear;
            m_currentGear = 0;
        }
        else
        {
            m_currentGear = m_lastGear;
        }
    }

    public void Reverse(float p_backwardsInput)
    {
        if (m_currentGear != -1)
            return;

        if (p_backwardsInput < 0)
            m_currentRPM = (int)Mathf.Lerp(m_minRPM, m_maxRPM, Mathf.Abs(Input.GetAxis("Vertical")));

        foreach (WheelCollider wheel in m_motorWheels)
        {
            wheel.motorTorque = m_currentRPM / 4 * p_backwardsInput;
        }
    }
}
