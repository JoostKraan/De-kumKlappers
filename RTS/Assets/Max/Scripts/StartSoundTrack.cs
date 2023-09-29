using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSoundTrack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Play a sound with custom volume and pitch.
        SoundManager.instance.PlaySound("SoundTrack", 1, 0.5f, 0.8f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
