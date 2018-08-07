using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartDestroy : MonoBehaviour
{
    float CurrentTime = 0f;
    GameObject Heart;
    GameObject BrokenHeart;

    void Start()
    {
        Heart = transform.GetChild(0).gameObject;
        if (Heart == null)
        {
            Debug.LogError("Heart 못 찾음");
            return;
        }

        BrokenHeart = transform.GetChild(1).gameObject;
        if (BrokenHeart == null)
        {
            Debug.LogError("BrokenHeart 못 찾음");
            return;
        }
    }

    void Update()
    {
        CurrentTime += Time.deltaTime;

        // 위로 올라감
        if (CurrentTime < 1f)
        {
            transform.position += (Vector3.forward / 2) * Time.deltaTime;
        }

        // 뽀각!
        if (CurrentTime > 2f)
        {
            Heart.SetActive(false);
            BrokenHeart.SetActive(true);

            enabled = false; // 자신 끔
        }

    }
}
