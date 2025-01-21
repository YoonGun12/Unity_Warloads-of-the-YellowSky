using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange; //스캔 범위
    public LayerMask targetLayer; //타겟 레이어(스캔할 레이어)
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    private void FixedUpdate()
    {
        //벙위 내에서 타겟을 탐지
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        //가장 가까운 타겟을 찾음
        nearestTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null; //결과를 저장할 변수
        float diff = 100; //가장 가까운 거리의 초기값(임의로 큰값으로 설정)

        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;//현재 위치
            Vector3 targetPos = target.transform.position;//타겟의 위치
            float curDiff = Vector3.Distance(myPos, targetPos); //현재 타겟과의 거리 계산

            //현재 타겟이 가장 가까운 경우
            if(curDiff < diff)
            {
                diff = curDiff; //거리 업데이트
                result = target.transform; //가장 가까운 타겟 업데이트
            }
        }

        return result; //가장 가까운 타겟 반환
    }
}
