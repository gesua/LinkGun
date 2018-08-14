using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePoint : MonoBehaviour
{
    Camera MainCam;

    void Start()
    {
        MainCam = Camera.main;

        Cursor.lockState = CursorLockMode.Confined; // 마우스 화면 안 벗어나게
        Cursor.visible = false; // 마우스 커서 안 보이게
    }

    void Update()
    {
        // 마우스 포인터가 마우스 따라다님
        Vector3 tempPos = MainCam.ScreenToWorldPoint(Input.mousePosition);
        tempPos.y = -10;
        transform.position = tempPos;
    }
}
