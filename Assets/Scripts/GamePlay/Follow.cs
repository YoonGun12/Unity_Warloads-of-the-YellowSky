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
        //플레이어의 월드 위치를 화면 좌표로 변환
        Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
        //UI 요소의 위치를 플레이어 화면 위치로 설정하고, 오프셋을 적용
        rect.position = playerScreenPos + offset; 
    }
}