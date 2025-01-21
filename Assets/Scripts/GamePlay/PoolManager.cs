using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    //������ ���� ����
    public GameObject[] prefabs;

    // �� �����տ� ���� ������ Ǯ ����Ʈ
    List<GameObject>[] pools;

    private void Awake()
    {
        //������ �迭�� ����ִ��� üũ
        if (prefabs == null || prefabs.Length == 0)
        {
            Debug.LogError("������ �迭�� ����ֽ��ϴ�!");
            return;
        }

        //������ �迭�� ���̿� ���� Ǯ ����Ʈ�� �ʱ�ȭ
        pools = new List<GameObject>[prefabs.Length];

        //�� �����տ� ���� ����ִ� ����Ʈ�� ����
        for(int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject> ();
        }

    }

    public GameObject Get(int index)
    {
        // �ε��� ��ȿ���� üũ
        if (index < 0 || index >= prefabs.Length)
        {
            Debug.LogError("�߸��� �ε����Դϴ�! index: " + index);
            return null;
        }

        // �������� null���� üũ
        if (prefabs[index] == null)
        {
            Debug.LogError("�������� null�Դϴ�! index: " + index);
            return null;
        }

        GameObject select = null;

        // Ǯ���� ��Ȱ��ȭ�� ��ü�� ã��
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                return select;  // ��ü�� ã���� �ٷ� ��ȯ
            }
        }

        // ��Ȱ��ȭ�� ��ü�� ������ ���� ����
        select = Instantiate(prefabs[index], transform);

        // ������ ��ü�� null�� �ƴ��� üũ �� Ǯ�� �߰�
        if (select != null)
        {
            pools[index].Add(select);
        }
        else
        {
            Debug.LogError("������ �ν��Ͻ�ȭ�� �����߽��ϴ�! index: " + index);
        }


        return select; //���� ������ ��ü ��ȯ
    }

}
