using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PelotaBehaviour : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb= GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        MainGameManager.LanzarPelota += LanzarPelota;
    }
    

    private void LanzarPelota(float holdTime)
    {
        rb.AddForce(Vector3.up * holdTime * 500f);
    }
}
