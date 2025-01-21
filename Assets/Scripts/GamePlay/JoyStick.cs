using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour
{
    public GameObject joystickBase; //조이스틱 기본 배경
    public GameObject joystickHandle; //조이스틱 레버
    public float joystickRadius = 100f; //조이스틱 최대 반경
    private Vector2 joystickStartPos; //조이스틱이 시작된 위치
    private Vector2 inputVector; //조이스틱 입력 벡터

    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;

    private void Awake()
    {
        // 게임 시작 시 조이스틱 배경을 비활성화
        joystickBase.SetActive(false);

        // 현재 Canvas에 있는 GraphicRaycaster와 EventSystem 가져오기
        raycaster = FindObjectOfType<Canvas>().GetComponent<GraphicRaycaster>();
        eventSystem = FindObjectOfType<EventSystem>();
    }

    private void Update()
    {
        // 게임이 활성화된 상태에서만 입력 처리
        if (!GameManager.instance.isLive)
        {
            return;
        }

        // 터치 입력 처리
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // UI를 터치한 경우, 조이스틱 비활성화 유지
            

            if (touch.phase == TouchPhase.Began)
            {
                if (IsPointerOverUI(touch.position))
                {
                    joystickBase.SetActive(false);
                    inputVector = Vector2.zero;
                    return;
                }

                // 터치가 시작되었을 때 조이스틱의 시작 위치를 저장, 조이스틱 배경 활성화
                joystickStartPos = touch.position;
                joystickBase.SetActive(true);
                joystickBase.transform.position = touch.position; // 조이스틱 배경의 위치를 터치 위치로 설정
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if(joystickBase.activeSelf)
                {
                    // 터치가 이동 중일 때 조이스틱 핸들의 위치를 업데이트
                    Vector2 direction = touch.position - joystickStartPos; // 시작 위치와 현재 터치 위치 간의 방향 계산
                    float distance = Mathf.Clamp(direction.magnitude, 0, joystickRadius); // 거리 제한
                    inputVector = direction.normalized * (distance / joystickRadius); // 입력 벡터 계산
                    joystickHandle.transform.position = joystickStartPos + inputVector * joystickRadius; // 핸들 위치 업데이트
                }
                
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                // 터치가 끝났을 때 조이스틱 배경과 핸들을 비활성화하고 입력 벡터를 초기화
                joystickBase.SetActive(false);
                inputVector = Vector2.zero;
            }
        }
    }

    private bool IsPointerOverUI(Vector2 touchPosition)
    {
        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = touchPosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        // UI 요소가 Raycast 결과에 포함되어 있는지 확인
        return results.Count > 0;
    }

    // 외부에서 입력 벡터를 조회할 수 있도록 반환하는 메서드
    public Vector2 GetInputVector()
    {
        return inputVector;
    }

    public float GetInputMagnitude()
    {
        return inputVector.magnitude;
    }
}
