using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainGameManager : MonoBehaviour
{
    public float ballSpeed = 10f;
    private float holdTime = 0f;
    private bool isTriggerHolding = false;
    private bool onBeat = false;

    //Estos eventos solo se triggerean cuando el jugador presiona y suelta en beat, respectivamente. 
    public static event Action<float> TriggerOnBeatStart;
    public static event Action<float> TriggerOnBeatEnd;


    void Start()
    {
        
    }
    private void OnEnable()
    {
        //Me suscribo al evento OnBeat del Conductor
        Conductor.OnBeat += SetOnBeat;
        //Me suscribo a los eventos de los trigger
        PlayerController.OnTriggerHoldStart += OnTriggerHoldStart;
        PlayerController.OnTriggerHoldEnd += OnTriggerHoldEnd;
    }

    /// <summary>
    /// Cambia el valor del booleano OnBeat de esta clase en razón del conductor. 
    /// </summary>
    /// <param name="isOnBeat"></param>
    private void SetOnBeat(bool isOnBeat)
    {
        onBeat= isOnBeat;
    }


    private void OnTriggerHoldStart(float startTime)
    {
        if (onBeat)
        {
            Debug.Log("Trigger presionado a tiempo");
            isTriggerHolding = true;
            TriggerOnBeatStart?.Invoke(startTime);
        }

    }
    private void OnTriggerHoldEnd(float holdTime)
    {
        if(onBeat && isTriggerHolding)
        {
            Debug.Log("Trigger soltado a tiempo. Holdtime: " + holdTime + " segundos");
            this.holdTime = holdTime;
            isTriggerHolding = false;
            TriggerOnBeatEnd?.Invoke(holdTime);
        }
        else
        {
            TriggerOnBeatEnd?.Invoke(-1);
        }
        

    }

}
