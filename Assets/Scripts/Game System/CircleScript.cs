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
    private bool isGrowing = false;
    private float holdTime;
    private Vector3 originalSize;
    [SerializeField] private GameObject indicadorExito;

    private void OnEnable()
    {
        MainGameManager.TriggerOnBeatStart += StartGrowing;
        MainGameManager.TriggerOnBeatEnd += StopGrowing;

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

    private void StartGrowing(float startTime)
    {
        isGrowing= true;
    }

    private void StopGrowing(float holdTime)
    {
        this.holdTime = holdTime;
        isGrowing = false;
    }

    private void WasOnBeat()
    {
        if (isGrowing)
        {
            Vector3 changeInSize = new Vector3(sizeToChange, sizeToChange, 0);
            gameObject.transform.localScale += changeInSize * Time.deltaTime;
        }
        else
        {
            gameObject.transform.localScale = originalSize;
        }
    }

}
