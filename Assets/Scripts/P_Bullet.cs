using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Bullet : MonoBehaviour
{
    // 속도
    public float speed = 3f;

    private void Start()
    {
        speed = Random.Range(3f, 10f);

        // 사라짐
        Destroy(gameObject, Random.Range(3f, 10f));
    }

    private void Update()
    {
        Vector3 dir = transform.forward;

        // 날아감
        transform.position += dir * speed * Time.deltaTime;
    }
}
