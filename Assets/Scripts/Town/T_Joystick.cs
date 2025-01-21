using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class T_Joystick : MonoBehaviour
{
    public GameObject joystickBase;
    public GameObject joystickHandle;
    public float joystickRadius = 100f;
    Vector2 joystickStartPos;
    Vector2 inputVector;

    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;

    private void Awake()
    {
        joystickBase.SetActive(false);
    }

    private void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                //UI 터치 여부 확인
                if (IsPointerOverUI(touch.position))
                {
                    joystickBase.SetActive(false);
                    inputVector = Vector2.zero;
                    return;
                }
                //터치가 시작되었을 때 조이스틱 활성화
                joystickStartPos = touch.position;
                joystickBase.SetActive(true);
                joystickBase.transform.position = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (joystickBase.activeSelf)
                {
                    //좌우 방향으로만 움직임을 계산
                    Vector2 direction = touch.position - joystickStartPos;
                    direction.y = 0; //세로 이동 제거
                    float distance = Mathf.Clamp(direction.magnitude, 0, joystickRadius);
                    inputVector = direction.normalized * (distance / joystickRadius);
                    joystickHandle.transform.position = joystickStartPos + inputVector * joystickRadius;

                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
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

        return results.Count > 0;

    }

    public float GetHorizontalInput()
    {
        return inputVector.x;
    }
}
