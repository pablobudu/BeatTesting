using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private void OnEnable()
    {
        Conductor.OnBeat += RecibirInput;
    }

    private void OnDisable()
    {
        Conductor.OnBeat -= RecibirInput;
    }

    public void RecibirInput()
    {

    }
}
