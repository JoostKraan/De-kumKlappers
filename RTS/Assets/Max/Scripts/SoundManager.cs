using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioMixerGroup masterMixerGroup; // Create an Audio Mixer in Unity and assign it here.

    [Header("Sound Groups")]
    public SoundGroup[] soundGroups;

    [System.Serializable]
    public class SoundGroup
    {
        public string groupName;
        public AudioClip[] audioClips;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0f, 2f)] public float pitch = 1f;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Play a sound from a specific group with optional volume and pitch settings.
    public void PlaySound(string groupName, int clipIndex = 0, float volume = 1f, float pitch = 1f)
    {
        SoundGroup group = FindSoundGroup(groupName);
        if (group != null && clipIndex >= 0 && clipIndex < group.audioClips.Length)
        {
            GameObject soundObject = new GameObject("SoundObject");
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = group.audioClips[clipIndex];
            audioSource.volume = volume * group.volume;
            audioSource.pitch = pitch * group.pitch;
            audioSource.outputAudioMixerGroup = masterMixerGroup;
            audioSource.Play();

            // Attach the AudioSource to the SoundManager GameObject.
            soundObject.transform.SetParent(transform);

            // Destroy the temporary GameObject when the sound finishes playing.
            Destroy(soundObject, audioSource.clip.length);
        }
    }

    private SoundGroup FindSoundGroup(string groupName)
    {
        foreach (SoundGroup group in soundGroups)
        {
            if (group.groupName == groupName)
                return group;
        }
        Debug.LogWarning("Sound group " + groupName + " not found.");
        return null;
    }
}
