using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    //Song beats per minute
    //This is determined by the song you're trying to sync up to
    public float songBpm;

    //The number of seconds for each song beat
    public float secPerBeat;

    //Current song position, in seconds
    public float songPosition;

    //Current song position, in beats
    public float songPositionInBeats;

    //How many seconds have passed since the song started
    public float dspSongTime;

    /// <summary>
    /// Esta variable nos permite darle más o menos tolerancia a lo que consideramos OnBeat
    /// </summary>
    [SerializeField] private float beatOffset = 0.3f;


    //an AudioSource attached to this GameObject that will play the music.
    private AudioSource musicSource;


    //Evento que permite que el control funcione
    public static event Action<bool> OnBeat;

    

    void Start()
    {
        //Load the AudioSource attached to the Conductor GameObject
        musicSource = GetComponent<AudioSource>();

        //Calculate the number of seconds in each beat
        secPerBeat = 60f / songBpm;

        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;

        //Start the music
        musicSource.Play();
    }
    void Update()
    {
        //determine how many seconds since the song started
        songPosition = (float)(AudioSettings.dspTime - dspSongTime);

        //determine how many beats since the song started
        songPositionInBeats = songPosition / secPerBeat;


        // Check if it's a beat and notify subscribers
        if (IsBeat())
        {
            OnBeat?.Invoke(true);
        }
        else
        {
            OnBeat?.Invoke(false);
        }
    }

    // Check if the current position is a beat
    private bool IsBeat()
    {
        // Calculate the fractional part of the current beat
        float beatFraction = songPositionInBeats - Mathf.Floor(songPositionInBeats);

        // Check if the fractional part is within a very small range
        // This indicates that we are very close to the beat
        if (beatFraction < beatOffset) // Adjust this threshold as needed
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
