using System;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public float songBpm;
    private AudioSource musicSource;

    private void Start()
    {
        musicSource = GetComponent<AudioSource>();
        musicSource.Play();
    }

    public float GetSongPosition()
    {
        return (float)(AudioSettings.dspTime - musicSource.timeSamples / musicSource.clip.frequency);
    }

    public float GetBpm()
    {
        return songBpm;
    }
}