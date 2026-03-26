using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이동만 책임
// 필수 컴포넌트 지정, 스크립트 추가 시 해당 컴포넌트 자동 추가
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;

    [Header("# Movement")]
    public float speed = 5f;
    public float gravity = -9.8f;

    private Vector3 velocity;
    private Vector2 inputVec;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
    }

    public void SetInput(Vector2 _input)
    {
        inputVec = _input;
    }

    void Move()
    {
        // 받아온 입력값으로 Vector3 형태의 이동 방향 벡터 생성
        Vector3 moveVec = new Vector3(inputVec.x, 0, inputVec.y);

        // 이동 방향 벡터의 크기가 1보다 크면 정규화하여 크기를 1로 만듦 -> 대각선 이동
        if (moveVec.magnitude > 1f)
            moveVec.Normalize();

        // 이동
        controller.Move(moveVec * speed * Time.deltaTime);

        // 중력 처리
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;
        
        // 중력 가속도 적용
        velocity.y += gravity * Time.deltaTime;
        // 중력 적용된 속도로 이동
        controller.Move(velocity * Time.deltaTime);

        // 방향 회전
        if (moveVec != Vector3.zero)
            transform.forward = moveVec;
    }
}
