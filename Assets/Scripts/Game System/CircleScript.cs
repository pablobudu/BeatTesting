using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

/// <summary>
/// A esta clase le concierne el crecimiento del circulo y su coloración
/// </summary>
public class CircleScript : MonoBehaviour
{

    public float sizeToChange = 2f;
    private bool isShrinking = false;
    public float minSize = 1f;
    private float holdTime;
    private Vector3 originalSize;
    private int beatCounter = 0;
    [SerializeField] private GameObject indicadorExito;

    private void OnEnable()
    {
        MainGameManager.TriggerOnBeatStart += StartShrinking;
        MainGameManager.TriggerOnBeatEnd += StopShrinking;
        Conductor.OnBeat += BeatCounting;

    }

    void Start()
    {
        originalSize = gameObject.transform.localScale;
        Vector3 changeInSize = new Vector3(0, 0, 0);
    }

    void Update()
    {
        WasOnBeat();

    }

    private void StartShrinking(float startTime)
    {
        isShrinking = true;
    }

    private void StopShrinking(float holdTime)
    {
        this.holdTime = holdTime;
        isShrinking = false;
    }

    private void WasOnBeat()
    {
        Vector3 currentSize = gameObject.transform.localScale;
        if (isShrinking && transform.localScale.magnitude >= minSize)
        {
            ChangeSize();
        }
        else 
        {
            gameObject.transform.localScale = originalSize;
        }
    }

    private void ChangeSize()
    {

        Vector3 changeInSize = new Vector3(sizeToChange, sizeToChange, 0);
        gameObject.transform.localScale -= changeInSize * Time.deltaTime;
    }
    //Cuando alcance el mínimo tamaño, esperar 2 beats antes de volver a resetear el tamaño y mostrar el indicador de éxito. 
    private void BeatCounting(bool isOnBeat)
    {
        if(isOnBeat)
        {
            
        }
    }

}
