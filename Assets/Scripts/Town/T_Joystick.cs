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
                //UI ��ġ ���� Ȯ��
                if (IsPointerOverUI(touch.position))
                {
                    joystickBase.SetActive(false);
                    inputVector = Vector2.zero;
                    return;
                }
                //��ġ�� ���۵Ǿ��� �� ���̽�ƽ Ȱ��ȭ
                joystickStartPos = touch.position;
                joystickBase.SetActive(true);
                joystickBase.transform.position = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (joystickBase.activeSelf)
                {
                    //�¿� �������θ� �������� ���
                    Vector2 direction = touch.position - joystickStartPos;
                    direction.y = 0; //���� �̵� ����
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
