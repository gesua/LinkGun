using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniTest : MonoBehaviour
{
    public Animator Ani;

    [Range(0, 1)]
    public float value = 0f;

    void Update()
    {
        Ani.Play("Idle", -1, value);
    }
}
