
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutlineColor : MonoBehaviour
{
    static Color PrevColor; // 이전 색
    static Color NextColor; // 다음 색
    static int ColorCount = -1;

    Outline MyOutline;

    private void Awake()
    {
        MyOutline = GetComponentInChildren<Outline>();
    }

    void Update()
    {
        MyOutline.effectColor = ColorChange();
    }

    Color RandomColor()
    {
        //float r = Random.Range(200f, 256f) / 255f;
        //float g = Random.Range(200f, 256f) / 255f;
        //float b = Random.Range(200f, 256f) / 255f;
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

        Color applyColor = Color.Lerp(PrevColor, NextColor, ColorCount / 20f);

        // 새로운 색 지정
        if (applyColor.Equals(NextColor))
        {
            PrevColor = NextColor;
            NextColor = RandomColor();
        }

        // 카운트 초기화
        if (ColorCount >= 20)
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
