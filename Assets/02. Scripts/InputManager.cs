using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 입력만 책임
public class InputManager : MonoBehaviour
{
    public PlayerController player;

    Vector2 startPos;
    Vector2 inputVec;

    private void Update()
    {
        HandleInput();
    }

    /// <summary>
    /// 마우스 입력을 처리하여 플레이어 컨트롤러에 전달하는 메서드
    /// </summary>
    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
            // 마우스 버튼이 처음 눌린 위치를 시작 위치로 저장
            startPos = Input.mousePosition;
        else if (Input.GetMouseButton(0))
        {
            // 현재 마우스 위치에서 시작 위치를 뺀 벡터가 입력 벡터
            Vector2 curPos = Input.mousePosition;
            Vector2 delta = curPos - startPos;

            inputVec = delta.normalized; // 입력 벡터를 정규화하여 방향만 사용

            // 플레이어 컨트롤러에 입력 벡터 전달
            player.SetInput(inputVec);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // 마우스 버튼이 떼어지면 입력 벡터 초기화
            inputVec = Vector2.zero;
            player.SetInput(inputVec);
        }
    }
}
