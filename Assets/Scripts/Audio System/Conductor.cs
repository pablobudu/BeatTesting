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
    private float secPerBeat;

    //Current song position, in seconds
    public float songPosition;

    //Current song position, in beats
    public float songPositionInBeats;

    //How many seconds have passed since the song started
    public float dspSongTime;

    /// <summary>
    /// Esta variable nos permite darle más o menos tolerancia a lo que consideramos OnBeat
    /// </summary>
    [SerializeField] private float beatOffset = 0.2f;


    //an AudioSource attached to this GameObject that will play the music.
    private AudioSource musicSource;

    private bool beatActionExecuted = false;


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
            if (!beatActionExecuted)
            {
                OnBeat?.Invoke(true);
                beatActionExecuted = true;
            }
        }
        else
        {
            beatActionExecuted = false;
            OnBeat?.Invoke(false);
        }
    }

    // Check if the current position is a beat
    private bool IsBeat()
    {
        // Calculate the beat index based on the current song position
        int currentBeatIndex = Mathf.FloorToInt(songPositionInBeats);

        // Calculate the absolute position of the current beat
        float currentBeatPosition = currentBeatIndex * secPerBeat;

        // Calculate the difference between the current position and the position of the last beat
        float timeSinceLastBeat = songPosition - currentBeatPosition;

        // Check if the difference is within the beatOffset
        if (Mathf.Abs(timeSinceLastBeat) < beatOffset)
        {
            
            return true;
        }
        else
        {
            return false;
        }
    }

    public float GetSecPerBeat()
    {
        return secPerBeat;
    }
}
