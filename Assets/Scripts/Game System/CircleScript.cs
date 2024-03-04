using System;
using UnityEngine;

/// <summary>
/// Controla el crecimiento y color del círculo.
/// </summary>
public class CircleScript : MonoBehaviour
{
    // Tamaño al que el círculo cambiará
    public float sizeToChange = 2f;
    private bool isShrinking = false;
    public float minSize = 1.5f;
    private float holdTime;
    private Vector3 originalSize;
    [SerializeField] private int beatCounter = 0;
    private bool isCountingBeats = false;
    private float timePerBeat;
    private Renderer renderer;
    public Color minSizeColor = Color.green;
    [SerializeField] private GameObject indicadorExito;
    private bool isInsideCollider;

    // Evento activado cuando la pelota está en el collider
    public static event Action<bool> IsTriggeringCollider;

    // Evento activado cuando el jugador es penalizado
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
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (isShrinking)
        {
            Shrink();
        }
        else
        {
            ResetSize();
            isCountingBeats = false;
        }
        OnBeatLimitReached();

        // Invocar evento si está dentro del collider
        IsTriggeringCollider?.Invoke(isInsideCollider);
    }

    // Iniciar encogimiento cuando comienza el beat
    private void StartShrinking(float startTime)
    {
        timePerBeat = FindObjectOfType<Conductor>().GetSecPerBeat();
        isShrinking = true;
    }

    // Detener encogimiento cuando termina el beat
    private void StopShrinking(float holdTime)
    {
        this.holdTime = holdTime;
        isShrinking = false;
    }

    // Encoger el círculo
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

    // Reducir el tamaño del círculo
    private void ReduceSize()
    {
        float timeToShrink = timePerBeat * 3;
        float sizeReductionPerSecond = (originalSize.magnitude - minSize) / timeToShrink;

        Vector3 changeInSize = new Vector3(sizeReductionPerSecond, sizeReductionPerSecond, 0);
        gameObject.transform.localScale -= changeInSize * Time.deltaTime;
    }

    // Restablecer el tamaño original del círculo
    private void ResetSize()
    {
        gameObject.transform.localScale = originalSize;
        ChangeColor(Color.blue);
    }

    // Contar los beats
    private void BeatCounting(bool isOnBeat)
    {
        if (isOnBeat && isCountingBeats)
        {
            beatCounter++;
        }
        else if (!isCountingBeats)
        {
            beatCounter = 0;
        }
    }

    // Manejar el límite de beats
    private void OnBeatLimitReached()
    {
        if (beatCounter == 2)
        {
            WasPenalized?.Invoke();
            HideCircle();
        }
        else if (beatCounter >= 3)
        {
            ShowCircle();
        }
    }

    // Cambiar el color del círculo
    private void ChangeColor(Color color)
    {
        renderer.material.color = color;
    }

    // Ocultar el círculo y desactivar el collider
    private void HideCircle()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
        ResetSize();
        isShrinking = false;
        indicadorExito.SetActive(true);
        isCountingBeats = true;
    }

    // Mostrar el círculo y activar el collider
    private void ShowCircle()
    {
        gameObject.GetComponent<Renderer>().enabled = true;
        gameObject.GetComponent<Collider>().enabled = true;
        indicadorExito.SetActive(false);
    }

    // Entrar al collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pelota"))
        {
            isInsideCollider = true;
        }
    }

    // Salir del collider
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pelota"))
        {
            isInsideCollider = false;
        }
    }
}
