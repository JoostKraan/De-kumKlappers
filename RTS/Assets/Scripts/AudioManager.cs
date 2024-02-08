using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    //public AudioSource[] m_AudioClip;
    public List<AudioSource> m_AudioSources = new List<AudioSource>();

    void Start() {
        m_AudioSources[0].Play();
    }
}
