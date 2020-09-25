using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAudio : MonoBehaviour
{
    public AudioClip m_idleClip;
    public AudioClip m_lowRPMClip;
    public AudioClip m_highRPMClip;

    private VehicleController m_controller;
    private AudioSource m_idle;
    private AudioSource m_lowRPM;
    private AudioSource m_highRPM;

    void Start()
    {
        m_controller = GetComponent<VehicleController>();

        m_idle = gameObject.AddComponent<AudioSource>();
        m_lowRPM = gameObject.AddComponent<AudioSource>();
        m_highRPM = gameObject.AddComponent<AudioSource>();


        m_idle.clip = m_idleClip;
        m_lowRPM.clip = m_lowRPMClip;
        m_highRPM.clip = m_highRPMClip;

        m_idle.loop = true;
        m_lowRPM.loop = true;
        m_highRPM.loop = true;

        m_idle.Play();
        m_lowRPM.Play();
        m_highRPM.Play();
    }


    void Update()
    {
        float ratio = ((float)m_controller.m_currentRPM / (float)m_controller.m_maxRPM);

        m_highRPM.volume = Mathf.Lerp(0f, 1f, ratio);
        m_highRPM.pitch = ratio;

        m_lowRPM.volume = Mathf.Lerp(1f, 0f, ratio);
        m_lowRPM.pitch = ratio + 0.3f;

        if (ratio < 0.25f)
        {
            m_lowRPM.volume = Mathf.Lerp(0f, 1f, ratio * 4);
            m_idle.volume = Mathf.Lerp(1f, 0f, ratio * 4);
            m_highRPM.volume = 0f;
        }
        else
            m_idle.volume = 0f;
    }
}
