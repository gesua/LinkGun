using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Bullet : MonoBehaviour
{
    // 속도
    public float speed = 10f;

    // 파워
    public int power = 1;

    private void Start()
    {
        // 3초 뒤 사라짐
        Destroy(gameObject, 3f);
    }

    private void Update()
    {
        Vector3 dir = transform.forward;

        // 날아감
        transform.position += dir * speed * Time.deltaTime;
    }
}
