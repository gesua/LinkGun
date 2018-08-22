using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    // 반짝반짝
    SpriteRenderer SR;
    int AddValue = 1;
    float ChangeValue = 1;
    bool IsStart = false;

    private void Start()
    {
        SR = GetComponent<SpriteRenderer>();

        if (SR == null)
        {
            Debug.LogError("EffectSR 못 찾음");
            return;
        }
    }

    void Update()
    {
        // 시작
        if (Input.anyKeyDown)
        {
            StartCoroutine("StartButton");
        }

        if (IsStart == false)
        {
            // 반짝반짝
            ChangeValue += AddValue * Time.deltaTime;

            if (ChangeValue <= 0.5f)
            {
                AddValue = 1;
            }
            else if (ChangeValue >= 1f)
            {
                AddValue = -1;
            }

            SR.color = new Color(1, 1, 1, ChangeValue);
        }
    }

    // 글자 빠르게 깜빡깜빡
    IEnumerator StartButton()
    {
        IsStart = true;
        SR.color = new Color(1, 1, 1, 1);

        for (int i = 0; i < 6; i++)
        {
            yield return new WaitForSeconds(0.1f);

            SR.enabled = !SR.enabled;
        }

        SceneManager.LoadScene("Main");
    }
}
