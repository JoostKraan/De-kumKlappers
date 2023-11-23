using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] soundtracks;
    private int currentTrackIndex = 0;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayNextTrack();
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextTrack();
        }
    }

    void PlayNextTrack()
    {
        if (soundtracks.Length > 0)
        {
            audioSource.clip = soundtracks[currentTrackIndex];
            audioSource.Play();
            currentTrackIndex = (currentTrackIndex + 1) % soundtracks.Length;
        }
    }
}
