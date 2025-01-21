using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint; // 적이 생성될 위치
    public List<SpawnPhase> spawnPhases; // 각 단계별로 다른 SpawnData 설정

    float timer; // 스폰 타이머
    int currentPhase = 0; // 현재 스폰 단계

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    private void Update()
    {
        if (!GameManager.instance.isLive)
        {
            return; // 게임이 진행 중이지 않으면 업데이트 중지
        }

        timer += Time.deltaTime; // 타이머 업데이트

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

        if (elapsed >= 210) currentPhase = 3; // 15분 이후
        else if (elapsed >= 180) currentPhase = 2; // 10~15분
        else if (elapsed >= 60) currentPhase = 1; // 5~10분
        else currentPhase = 0; // 0~5분
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
    public List<SpawnData> spawnData; // 이 단계에서 사용할 SpawnData 리스트
}

[System.Serializable]
public class SpawnData
{
    public int spriteType; // 적 스프라이트 타입    
    public int health; // 적 체력
    public float speed; // 적 속도    
    public float spawnTime; // 스폰 주기
    public int spawnAmount; // 스폰할 적의 수
}
