using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDisplay : MonoBehaviour
{
    public GameObject m_player;
    public GameObject m_speedText;
    public GameObject m_gearText;

    private TMP_Text m_speed;
    private TMP_Text m_gear;
    private VehicleController m_controller;

    void Start()
    {
        m_speed = m_speedText.GetComponent<TMP_Text>();
        m_gear = m_gearText.GetComponent<TMP_Text>();
        m_controller = m_player.GetComponent<VehicleController>();
    }

    void Update()
    {
        m_speed.text = "Speed: " + (int)m_controller.m_speed;
        m_gear.text = "Gear: " + m_controller.m_currentGear + "   " + m_controller.m_currentRPM;
    }
}
