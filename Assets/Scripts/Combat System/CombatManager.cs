using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    //Esto está feo pero hay que verlo. Tengo que inyectarlo para utilizarlo aki
    
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
    private void Update()
    {
        RecibirInput();
    }
    public void RecibirInput()
    {
        if(playerController.IsActionPressed())
        {
            Debug.Log("Presionado a tiempo.");
        }
    }
}
