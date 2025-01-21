using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logo : MonoBehaviour
{
    public RectTransform logoRect;
    public Vector2 startPos; //�ΰ��� ������ġ
    public Vector2 targetPos; //�ΰ� ������ ��ǥ ��ġ
    public float smoothTime = 0.5f; //�̵� �ӵ� ���� ��

    public GameObject menuButtons;

    Vector2 currentVelocity = Vector2.zero; //���� �ӵ�
    bool isSkipped = false;

    private void Start()
    {
        logoRect.anchoredPosition = startPos;
        menuButtons.SetActive(false);
        StartCoroutine(MoveLogo());
    }

    IEnumerator MoveLogo()
    {
        while (Vector2.Distance(logoRect.anchoredPosition, targetPos) > 0.1f && !isSkipped)
        {
            logoRect.anchoredPosition = Vector2.SmoothDamp(
                logoRect.anchoredPosition,
                targetPos,
                ref currentVelocity,
                smoothTime
                );

            yield return null;
        }

        if(isSkipped)
        {
            logoRect.anchoredPosition = targetPos;
        }

        logoRect.anchoredPosition = targetPos;

        menuButtons.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            isSkipped = true;
        }
    }
}
