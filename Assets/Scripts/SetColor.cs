using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColor : MonoBehaviour
{
    static Color PrevColor; // 이전 색
    static Color NextColor; // 다음 색
    static int ColorCount = -1;

    void Start()
    {
        GetComponentInChildren<SpriteRenderer>().color = ColorChange();
    }

    public Color RandomColor()
    {
        //float r = Random.Range(0f, 200f) / 255f;
        //float g = Random.Range(0f, 200f) / 255f;
        //float b = Random.Range(0f, 200f) / 255f;
        float r = Random.value;
        float g = Random.value;
        float b = Random.value;

        return new Color(r, g, b);
    }

    // 색 보간
    Color ColorChange()
    {
        // 맨 처음에만 지정
        if (ColorCount == -1)
        {
            PrevColor = RandomColor();
            NextColor = RandomColor();
            ColorCount++;
        }

        Color applyColor = Color.Lerp(PrevColor, NextColor, ColorCount / 10f);

        // 새로운 색 지정
        if (applyColor.Equals(NextColor))
        {
            PrevColor = NextColor;
            NextColor = RandomColor();
        }

        // 카운트 초기화
        if (ColorCount >= 10)
        {
            ColorCount = 0;
        }
        else
        {
            ColorCount++;
        }

        return applyColor;
    }
}
