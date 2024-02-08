using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public List<AudioSource> m_AudioSources = new List<AudioSource>();

    void Start() {
        m_AudioSources[0].Play();
    }

    public void PlayClick() {
        m_AudioSources[1].Play();
    }
}
