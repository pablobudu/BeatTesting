using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Esta clase nos permite animar en sincronía con el beat. Hay que ponerselo a cada objeto que queramos animar. 
/// </summary>
public class SyncedAnimation : MonoBehaviour
{
    //Records the animation state or animation that the Animator is currently in
    public AnimatorStateInfo animatorStateInfo;

    //Used to address the current state within the Animator using the Play() function
    public int currentState;



}
