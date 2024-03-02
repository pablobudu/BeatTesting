using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerControls playerControls;
    private float holdStartTime = 0f;
    private bool isHolding = false;
    private float holdTime = 0f;

    public static event Action<float> OnTriggerHoldStart;
    public static event Action<float> OnTriggerHoldEnd;


    void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        IsActionPressed();
        if(isHolding)
        {
            UpdateHoldTime();
        }
        
    }


    public bool IsActionPressed()
    {
        if (playerControls.Player.ActionButton.triggered)
        {
            Debug.Log("Presionado");
            return true;
        }
        return false;

    }

    #region Hold System
    public void OnTriggerHold(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            StartHolding();
        }
        else if(context.canceled)
        {
            StopHolding();
        }
    }

    private void StartHolding()
    {
        holdStartTime = Time.time;
        isHolding = true;
        OnTriggerHoldStart?.Invoke(holdStartTime);
    }
    private void StopHolding()
    {
        isHolding= false;
        OnTriggerHoldEnd?.Invoke(holdTime);
        holdTime = 0f;
        
    }
    private void UpdateHoldTime()
    {
        holdTime = Time.time - holdStartTime;
        
    }
    #endregion
}
