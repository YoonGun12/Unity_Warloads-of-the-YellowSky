using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    //프리펩 보관 변수
    public GameObject[] prefabs;

    // 각 프리팹에 대해 관리할 풀 리스트
    List<GameObject>[] pools;

    private void Awake()
    {
        //프리팹 배열이 비어있는지 체크
        if (prefabs == null || prefabs.Length == 0)
        {
            Debug.LogError("프리팹 배열이 비어있습니다!");
            return;
        }

        //프리펩 배열의 길이에 맞춰 풀 리스트를 초기화
        pools = new List<GameObject>[prefabs.Length];

        //각 프리팹에 대해 비어있는 리스트를 생성
        for(int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject> ();
        }

    }

    public GameObject Get(int index)
    {
        // 인덱스 유효범위 체크
        if (index < 0 || index >= prefabs.Length)
        {
            Debug.LogError("잘못된 인덱스입니다! index: " + index);
            return null;
        }

        // 프리팹이 null인지 체크
        if (prefabs[index] == null)
        {
            Debug.LogError("프리팹이 null입니다! index: " + index);
            return null;
        }

        GameObject select = null;

        // 풀에서 비활성화된 객체를 찾음
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                return select;  // 객체를 찾으면 바로 반환
            }
        }

        // 비활성화된 객체가 없으면 새로 생성
        select = Instantiate(prefabs[index], transform);

        // 생성된 객체가 null이 아닌지 체크 후 풀에 추가
        if (select != null)
        {
            pools[index].Add(select);
        }
        else
        {
            Debug.LogError("프리팹 인스턴스화에 실패했습니다! index: " + index);
        }


        return select; //새로 생성한 객체 반환
    }

}
