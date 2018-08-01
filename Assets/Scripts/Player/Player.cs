using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 이동속도
    public float speed = 3f;

    // 애니메이션 상태
    enum ANI_STATE
    {
        Idle,
        Move
    }
    ANI_STATE PrevState = ANI_STATE.Idle; // 이전 상태
    ANI_STATE NextState = ANI_STATE.Idle; // 다음 상태

    Animator PlayerAnimator;

    private void Start()
    {
        PlayerAnimator = GetComponentInChildren<Animator>();
        if (PlayerAnimator == null)
        {
            Debug.LogError("animator 못 찾음");
            return;
        }
    }

    void Update()
    {
        // 이동중
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            NextState = ANI_STATE.Move;
            Move();
        }
        else
        {
            NextState = ANI_STATE.Idle;
        }

        PlayAnimator();
    }

    // 이동(WASD)
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

    // 애니메이터 재생
    void PlayAnimator()
    {
        if (PrevState != NextState)
        {
            PrevState = NextState;
            PlayerAnimator.SetInteger("State", (int)PrevState);
        }
    }
}
