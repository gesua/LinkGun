using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColor : MonoBehaviour
{
    void Start()
    {
        GetComponent<SpriteRenderer>().color = RandomColor();
    }

    public Color RandomColor()
    {
        float r = Random.Range(0f, 255f) / 255f;
        float g = Random.Range(0f, 255f) / 255f;
        float b = Random.Range(0f, 255f) / 255f;

        return new Color(r, g, b);
    }
}
