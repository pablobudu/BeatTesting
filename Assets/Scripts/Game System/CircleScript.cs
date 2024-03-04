using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

/// <summary>
/// A esta clase le concierne el crecimiento del circulo y su coloración
/// </summary>
public class CircleScript : MonoBehaviour
{

    //En algun momento refactorizar esta clase para que el conteo se haga de forma automática del 0 al 3. De forma que todo se base en los beats.
    //Entonces, el shrink dura 3 beats desde q se presiona, y tiense un beat extra para cargar. Si no alcanzas a soltarlo en el beat 4 te penaliza

    public float sizeToChange = 2f;
    private bool isShrinking = false;
    public float minSize = 1.5f;
    private float holdTime;
    private Vector3 originalSize;
    [SerializeField]private int beatCounter = 0;
    private bool isCountingBeats = false;
    private float timePerBeat;
    private Renderer renderer;
    public Color minSizeColor = Color.green;
    [SerializeField] private GameObject indicadorExito;
    private bool isInsideCollider;

    //Evento se triggerea cuando elemento Pelota está en el collider.
    public static event Action<bool> IsTriggeringCollider;

    //Este evento se ejecuta cuando el jugador fue penalizado. Lo utilizaré en MainGameManager para tenerlo en cuenta.
    public static event Action WasPenalized;

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
        renderer = GetComponent<Renderer>();
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
            isCountingBeats = false;
        }
        OnBeatLimitReached();

        //Si el booleano está true, entonces invocarlo.
        if (isInsideCollider)
        {
            IsTriggeringCollider?.Invoke(true);
        }
        else
        {
            IsTriggeringCollider?.Invoke(false);
        }
    }

    private void StartShrinking(float startTime)
    {
        //Sería bueno tener el tiempo exacto del beat, y restarlo al tiempo en que  se apreto. Ese valor restarlo en esta operación para que el shrink fuera lo más exacto posible. 
        timePerBeat = FindObjectOfType<Conductor>().GetSecPerBeat() ; 
        isShrinking = true;
    }

    private void StopShrinking(float holdTime)
    {
        this.holdTime = holdTime;
        isShrinking = false;
    }

    private void Shrink()
    {
        if (transform.localScale.magnitude > minSize)
        {
            ReduceSize();
        }
        else
        {
            ChangeColor(minSizeColor);
            isCountingBeats = true;
        }
    }

    private void ReduceSize()
    {
        float timeToShrink = timePerBeat * 3; // Tiempo necesario para encoger después de 3 beats
        float sizeReductionPerSecond = (originalSize.magnitude - minSize) / timeToShrink; //1) Obtenemos cuanto hay que reducir, y eso lo dividimos x el tiempo.

        Vector3 changeInSize = new Vector3(sizeReductionPerSecond, sizeReductionPerSecond, 0);
        gameObject.transform.localScale -= changeInSize * Time.deltaTime;
    }


    private void ResetSize()
    {
        gameObject.transform.localScale = originalSize;
        ChangeColor(Color.blue);
    }


    //Cuando alcance el mínimo tamaño comenzamos a contar. Cuando no esté en el mínimo volvemos el contador a 0. 
    private void BeatCounting(bool isOnBeat)
    {
        if(isOnBeat && isCountingBeats)
        {
            beatCounter++;
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
            WasPenalized?.Invoke();
            Renderer renderer = gameObject.GetComponent<Renderer>(); // Obtén el componente Renderer del gameobject
            renderer.enabled = false; // Oculta el gameobject
            gameObject.GetComponent<Collider>().enabled = false; // Desactiva el collider
            ResetSize(); // Restablece el tamaño del gameobject
            isShrinking = false; // Detiene el encogimiento
            indicadorExito.SetActive(true); // Muestra el indicador de penalización
            isCountingBeats= true;
        }
        else if (beatCounter >=3)
        {
            Renderer renderer = gameObject.GetComponent<Renderer>(); // Obtén el componente Renderer del gameobject
            renderer.enabled = true; // Muestra el gameobject
            gameObject.GetComponent<Collider>().enabled = true; // Activa el collider
            indicadorExito.SetActive(false); // Oculta el indicador de penalización
        }
    }

    private void ChangeColor(Color color)
    {
        renderer.material.color = color; // Cambiar el color del material
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Pelota"))
        {
            isInsideCollider = true;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pelota"))
        {
            isInsideCollider = false;
        }
    }
}
