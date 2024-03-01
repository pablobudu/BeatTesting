using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        Conductor.OnBeat += RecibirInput;
    }

    private void OnDisable()
    {
        Conductor.OnBeat -= RecibirInput;
    }

    public void RecibirInput(bool isOnBeat)
    {
        if (isOnBeat && playerController.IsActionPressed())
        {
            Debug.Log("Presionado a tiempo.");
        }
    }
}
