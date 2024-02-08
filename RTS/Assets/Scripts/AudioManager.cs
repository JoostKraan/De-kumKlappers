using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public AudioSource[] m_AudioClip;

    void Start()
    {
        m_AudioClip[0].Play();
    }
}
