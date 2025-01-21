using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint; // ���� ������ ��ġ
    public List<SpawnPhase> spawnPhases; // �� �ܰ躰�� �ٸ� SpawnData ����

    float timer; // ���� Ÿ�̸�
    int currentPhase = 0; // ���� ���� �ܰ�

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    private void Update()
    {
        if (!GameManager.instance.isLive)
        {
            return; // ������ ���� ������ ������ ������Ʈ ����
        }

        timer += Time.deltaTime; // Ÿ�̸� ������Ʈ

        UpdatePhase();

        if (timer > spawnPhases[currentPhase].spawnData[0].spawnTime)
        {
            timer = 0;
            SpawnEnemies();
        }
    }

    void UpdatePhase()
    {
        float elapsed = GameManager.instance.gameTime;

        if (elapsed >= 210) currentPhase = 3; // 15�� ����
        else if (elapsed >= 180) currentPhase = 2; // 10~15��
        else if (elapsed >= 60) currentPhase = 1; // 5~10��
        else currentPhase = 0; // 0~5��
    }

    void SpawnEnemies()
    {
        var spawnDataArray = spawnPhases[currentPhase].spawnData;

        foreach (var spawnData in spawnDataArray)
        {
            for (int i = 0; i < spawnData.spawnAmount; i++)
            {
                GameObject enemy = GameManager.instance.pool.Get(spawnData.spriteType);
                Transform spawnPoint = this.spawnPoint[Random.Range(0, this.spawnPoint.Length)];
                enemy.transform.position = spawnPoint.position;
                enemy.GetComponent<EnemyMove>().Init(spawnData);
            }
        }
    }
}

[System.Serializable]
public class SpawnPhase
{
    public List<SpawnData> spawnData; // �� �ܰ迡�� ����� SpawnData ����Ʈ
}

[System.Serializable]
public class SpawnData
{
    public int spriteType; // �� ��������Ʈ Ÿ��    
    public int health; // �� ü��
    public float speed; // �� �ӵ�    
    public float spawnTime; // ���� �ֱ�
    public int spawnAmount; // ������ ���� ��
}
