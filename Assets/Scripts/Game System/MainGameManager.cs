using System;
using UnityEngine;

public class MainGameManager : MonoBehaviour
{
    // Velocidad de la pelota
    public float ballSpeed = 10f;

    // Tiempo de retenci�n actual y mejor tiempo de retenci�n
    private float holdTime = 0f;
    private float bestHoldTime = 0f;

    // Estado de retenci�n del disparador y estado del ritmo
    private bool isTriggerHolding = false;
    private bool onBeat = false;

    // Indicador si el jugador est� interactuando con el collider
    private bool isTriggeringColliderBool = false;

    // Referencia al objeto del jugador
    [SerializeField] private GameObject circlePlayer;

    // Eventos que se activan cuando el jugador presiona y suelta en el ritmo
    public static event Action<float> TriggerOnBeatStart;
    public static event Action<float> TriggerOnBeatEnd;

    // Evento que se activa cuando se lanza la pelota
    public static event Action<float> LanzarPelota;

    // Suscripci�n a eventos
    private void OnEnable()
    {
        Conductor.OnBeat += SetOnBeat;
        PlayerController.OnTriggerHoldStart += OnTriggerHoldStart;
        PlayerController.OnTriggerHoldEnd += OnTriggerHoldEnd;
        CircleScript.IsTriggeringCollider += IsTriggeringCollider;
        CircleScript.WasPenalized += PlayerWasPenalized;
    }

    // Cambia el estado del ritmo
    private void SetOnBeat(bool isOnBeat)
    {
        onBeat = isOnBeat;
    }

    // Detecta cuando el jugador presiona el gatillo
    private void OnTriggerHoldStart(float startTime)
    {
        if (onBeat)
        {
            isTriggerHolding = true;
            TriggerOnBeatStart?.Invoke(startTime);
        }
    }

    // Actualiza el tiempo de retenci�n y maneja la liberaci�n del gatillo
    private void OnTriggerHoldEnd(float holdTimeFromController)
    {
        if (onBeat && isTriggerHolding)
        {
            holdTime = holdTimeFromController;
            DevolverPelotaYUpdateHoldTime(holdTime);
            isTriggerHolding = false;
            TriggerOnBeatEnd?.Invoke(holdTimeFromController);
            holdTime = 0;
        }
        else
        {
            TriggerOnBeatEnd?.Invoke(-1);
        }
    }

    // Actualiza el mejor tiempo de retenci�n
    private void UpdateBestHoldTime(float currentHoldTime)
    {
        if (currentHoldTime > bestHoldTime)
        {
            bestHoldTime = currentHoldTime;
        }
    }

    // Determina si el collider est� siendo activado
    private void IsTriggeringCollider(bool isTriggering)
    {
        isTriggeringColliderBool = isTriggering;
    }

    // Devuelve la pelota si el jugador est� interactuando con el collider
    private void DevolverPelotaYUpdateHoldTime(float holdTime)
    {
        if (isTriggeringColliderBool)
        {
            
            LanzarPelota?.Invoke(holdTime); //Lanza la pelota. El comportamiento de esto estar� dentro de la pelota.
            UpdateBestHoldTime(holdTime); //
            Debug.Log("HoldTime actualizado: " + holdTime);
            Debug.Log("Best holdTime" + bestHoldTime);
        }
    }

    // Maneja la penalizaci�n del jugador
    private void PlayerWasPenalized()
    {
        Debug.Log("�Jugador penalizado!");
        holdTime = 0f;
        isTriggerHolding = false;
    }
}
