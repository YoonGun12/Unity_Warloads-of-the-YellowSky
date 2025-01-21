using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    RectTransform rect;

    public Vector3 offset;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        //�÷��̾��� ���� ��ġ�� ȭ�� ��ǥ�� ��ȯ
        Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
        //UI ����� ��ġ�� �÷��̾� ȭ�� ��ġ�� �����ϰ�, �������� ����
        rect.position = playerScreenPos + offset; 
    }
}