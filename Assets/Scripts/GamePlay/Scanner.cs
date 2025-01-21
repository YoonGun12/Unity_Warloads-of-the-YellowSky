using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange; //��ĵ ����
    public LayerMask targetLayer; //Ÿ�� ���̾�(��ĵ�� ���̾�)
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    private void FixedUpdate()
    {
        //���� ������ Ÿ���� Ž��
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        //���� ����� Ÿ���� ã��
        nearestTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null; //����� ������ ����
        float diff = 100; //���� ����� �Ÿ��� �ʱⰪ(���Ƿ� ū������ ����)

        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;//���� ��ġ
            Vector3 targetPos = target.transform.position;//Ÿ���� ��ġ
            float curDiff = Vector3.Distance(myPos, targetPos); //���� Ÿ�ٰ��� �Ÿ� ���

            //���� Ÿ���� ���� ����� ���
            if(curDiff < diff)
            {
                diff = curDiff; //�Ÿ� ������Ʈ
                result = target.transform; //���� ����� Ÿ�� ������Ʈ
            }
        }

        return result; //���� ����� Ÿ�� ��ȯ
    }
}
