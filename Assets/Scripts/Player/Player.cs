using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 이동속도
    public float speed = 3f;

    void Update()
    {
        Move();
    }

    // 플레이어 이동(WASD)
    void Move()
    {
        Vector3 moveVector = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveVector.z += speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveVector.z -= speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveVector.x -= speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveVector.x += speed * Time.deltaTime;
        }

        transform.position += moveVector;
    }
}
