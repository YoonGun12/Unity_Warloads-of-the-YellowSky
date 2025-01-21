using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour
{
    public GameObject joystickBase; //���̽�ƽ �⺻ ���
    public GameObject joystickHandle; //���̽�ƽ ����
    public float joystickRadius = 100f; //���̽�ƽ �ִ� �ݰ�
    private Vector2 joystickStartPos; //���̽�ƽ�� ���۵� ��ġ
    private Vector2 inputVector; //���̽�ƽ �Է� ����

    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;

    private void Awake()
    {
        // ���� ���� �� ���̽�ƽ ����� ��Ȱ��ȭ
        joystickBase.SetActive(false);

        // ���� Canvas�� �ִ� GraphicRaycaster�� EventSystem ��������
        raycaster = FindObjectOfType<Canvas>().GetComponent<GraphicRaycaster>();
        eventSystem = FindObjectOfType<EventSystem>();
    }

    private void Update()
    {
        // ������ Ȱ��ȭ�� ���¿����� �Է� ó��
        if (!GameManager.instance.isLive)
        {
            return;
        }

        // ��ġ �Է� ó��
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // UI�� ��ġ�� ���, ���̽�ƽ ��Ȱ��ȭ ����
            

            if (touch.phase == TouchPhase.Began)
            {
                if (IsPointerOverUI(touch.position))
                {
                    joystickBase.SetActive(false);
                    inputVector = Vector2.zero;
                    return;
                }

                // ��ġ�� ���۵Ǿ��� �� ���̽�ƽ�� ���� ��ġ�� ����, ���̽�ƽ ��� Ȱ��ȭ
                joystickStartPos = touch.position;
                joystickBase.SetActive(true);
                joystickBase.transform.position = touch.position; // ���̽�ƽ ����� ��ġ�� ��ġ ��ġ�� ����
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if(joystickBase.activeSelf)
                {
                    // ��ġ�� �̵� ���� �� ���̽�ƽ �ڵ��� ��ġ�� ������Ʈ
                    Vector2 direction = touch.position - joystickStartPos; // ���� ��ġ�� ���� ��ġ ��ġ ���� ���� ���
                    float distance = Mathf.Clamp(direction.magnitude, 0, joystickRadius); // �Ÿ� ����
                    inputVector = direction.normalized * (distance / joystickRadius); // �Է� ���� ���
                    joystickHandle.transform.position = joystickStartPos + inputVector * joystickRadius; // �ڵ� ��ġ ������Ʈ
                }
                
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                // ��ġ�� ������ �� ���̽�ƽ ���� �ڵ��� ��Ȱ��ȭ�ϰ� �Է� ���͸� �ʱ�ȭ
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

        // UI ��Ұ� Raycast ����� ���ԵǾ� �ִ��� Ȯ��
        return results.Count > 0;
    }

    // �ܺο��� �Է� ���͸� ��ȸ�� �� �ֵ��� ��ȯ�ϴ� �޼���
    public Vector2 GetInputVector()
    {
        return inputVector;
    }

    public float GetInputMagnitude()
    {
        return inputVector.magnitude;
    }
}
