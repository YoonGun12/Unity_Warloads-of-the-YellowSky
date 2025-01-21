using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class T_SwipeStage : MonoBehaviour
{
    public GameObject stageSelectScroll;
    float scrollPos = 0; //현재 스크롤 위치
    float[] pos; //자식의 스크롤 위치를 저장할 배열

    private void Update()
    {
        pos = new float[transform.childCount];
        float distance =  1f/(pos.Length-1); //콘텐츠 간의 거리 계산

        for(int i = 0; i < pos.Length; i++) // 각 자식요소의 스크롤 위치 계산
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
            //마우스를 놓으면 가장 가까운 위치로 스크롤
            for(int i = 0; i < pos.Length; i++)
            {
                //현재 스크롤 위치가 특정 콘텐츠의 중심에 가까울 경우
                if(scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
                {
                    stageSelectScroll.GetComponent<Scrollbar>().value = Mathf.Lerp(stageSelectScroll.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }

        //콘텐츠 크기 조정
        for (int i = 0; i < pos.Length; i++)
        {
            RectTransform rect = transform.GetChild(i).GetComponent<RectTransform>();

            if (scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
            {
                //해당 콘텐츠를 점진적으로 확대
                rect.sizeDelta = Vector2.Lerp(rect.sizeDelta, new Vector2(350, 500), 0.1f);
                //다른 콘텐츠는 작게 설정
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
