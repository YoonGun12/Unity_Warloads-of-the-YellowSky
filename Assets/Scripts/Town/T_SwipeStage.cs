using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class T_SwipeStage : MonoBehaviour
{
    public GameObject stageSelectScroll;
    float scrollPos = 0; //���� ��ũ�� ��ġ
    float[] pos; //�ڽ��� ��ũ�� ��ġ�� ������ �迭

    private void Update()
    {
        pos = new float[transform.childCount];
        float distance =  1f/(pos.Length-1); //������ ���� �Ÿ� ���

        for(int i = 0; i < pos.Length; i++) // �� �ڽĿ���� ��ũ�� ��ġ ���
        {
            pos[i] = distance * i;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Moved)
            {
                scrollPos = stageSelectScroll.GetComponent<Scrollbar>().value;
            }
        }
        else
        {
            //���콺�� ������ ���� ����� ��ġ�� ��ũ��
            for(int i = 0; i < pos.Length; i++)
            {
                //���� ��ũ�� ��ġ�� Ư�� �������� �߽ɿ� ����� ���
                if(scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
                {
                    stageSelectScroll.GetComponent<Scrollbar>().value = Mathf.Lerp(stageSelectScroll.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }

        //������ ũ�� ����
        for (int i = 0; i < pos.Length; i++)
        {
            RectTransform rect = transform.GetChild(i).GetComponent<RectTransform>();

            if (scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
            {
                //�ش� �������� ���������� Ȯ��
                rect.sizeDelta = Vector2.Lerp(rect.sizeDelta, new Vector2(350, 500), 0.1f);
                //�ٸ� �������� �۰� ����
                for (int a = 0; a < pos.Length; a++)
                {
                    if(a != i)
                    {
                        RectTransform otherRect = transform.GetChild(a).GetComponent<RectTransform>();
                        otherRect.sizeDelta = Vector2.Lerp(otherRect.sizeDelta, new Vector2(250, 400), 0.1f);
                    }
                }
            }
        }
    }
}
