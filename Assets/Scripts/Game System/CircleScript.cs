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
    public float minSize = 1.5f;
    private float holdTime;
    private Vector3 originalSize;
    private int beatCounter = 0;
    private bool isCountingBeats = false;
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
        if(isShrinking)
        {
            Shrink();
        }
        else
        {
            ResetSize();
        }


        
        OnBeatLimitReached();

       
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

    private void Shrink()
    {
        if (transform.localScale.magnitude >= minSize)
        {
          ReduceSize();
        }
        else
        {
            isCountingBeats = true;
        }

    }

    private void ReduceSize()
    {
        Vector3 changeInSize = new Vector3(sizeToChange, sizeToChange, 0);
        gameObject.transform.localScale -= changeInSize * Time.deltaTime;
    }
    private void ResetSize()
    {
        gameObject.transform.localScale = originalSize;
    }


    //Cuando alcance el mínimo tamaño comenzamos a contar. Cuando no esté en el mínimo volvemos el contador a 0. 
    private void BeatCounting(bool isOnBeat)
    {
        if(isOnBeat && isCountingBeats)
        {
            beatCounter++;
            Debug.Log(beatCounter);
        }else if(!isCountingBeats)
        {
            beatCounter = 0;
        }
    }

    /// <summary>
    /// Este se llama cuando se alcanzó el tamaño mínimo, y comenzó a correr el contador de beats. 
    /// Por tanto, cuando el beatCounter sea 2 (hayan pasado 2 beats) se mostrará el error y se penaliza al jugador con 1 beat. 
    /// </summary>
    private void OnBeatLimitReached()
    {
        if (beatCounter == 2)
        {
            Debug.Log("No lo soltaste a tiempo");
            Renderer renderer = gameObject.GetComponent<Renderer>(); // Obtén el componente Renderer del gameobject
            renderer.enabled = false; // Oculta el gameobject
            gameObject.GetComponent<Collider>().enabled = false; // Desactiva el collider
            ResetSize(); // Restablece el tamaño del gameobject
            isShrinking = false; // Detiene el encogimiento
            indicadorExito.SetActive(true); // Muestra el indicador de penalización
        }
        else if (beatCounter >= 3)
        {
            Renderer renderer = gameObject.GetComponent<Renderer>(); // Obtén el componente Renderer del gameobject
            renderer.enabled = true; // Muestra el gameobject
            gameObject.GetComponent<Collider>().enabled = true; // Activa el collider
            indicadorExito.SetActive(false); // Oculta el indicador de penalización
            isCountingBeats = false;
        }
    }
}
