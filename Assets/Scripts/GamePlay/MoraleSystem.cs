using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoraleSystem : MonoBehaviour
{
    public Image moraleBar;
    public float moraleDecayRate = 0.001f; //��� ������ ���� �ӵ�
    public float moraleGrowthRate = 0.1f; //���� ���� �� ��� ������ ���� �ӵ�
    float moraleValue; //���� ��� ������ �� (0~1)
    int currentPhase = 0; //���� ��� �ܰ�
    public bool isBossSpawned = false;
    private float[] healthMultipliers = { 1.0f, 1.2f, 1.5f, 2.0f };
    private float[] speedMultipliers = { 1.0f, 1.2f, 1.5f, 2.0f };

    private void Start()
    {
        moraleValue = 0f;
        moraleBar.fillAmount = moraleValue;
        StartCoroutine(DecayMorale());
        
    }

    private void Update()
    {
        if(!isBossSpawned && (moraleValue >= 1f || GameManager.instance.gameTime >= 180f))
        {
            moraleValue = 1f;
            CheckMoralePhase();
        }
    }

    IEnumerator DecayMorale()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f); //5�ʸ��� ����

            if(isBossSpawned )
            {
                IncreaseMorale(moraleGrowthRate);
            }
            else
            {
                DecreaseMorale(moraleDecayRate);
            }
            
        }
    }

    public void IncreaseMorale(float amount) //amount = �� óġ �� ��� ������ ������
    {
        if(!isBossSpawned)
        {
            moraleValue = Mathf.Clamp01(moraleValue + amount);
            moraleBar.fillAmount = moraleValue;
            CheckMoralePhase();
        }
        else
        {
            moraleValue = Mathf.Clamp01(moraleValue + amount);
            moraleBar.fillAmount = moraleValue;
            CheckMoralePhase();
        }
        
    }

    void DecreaseMorale(float amount)
    {
        moraleValue = Mathf.Clamp01(moraleValue - amount);
        moraleBar.fillAmount = moraleValue;
        CheckMoralePhase();
    }

    void CheckMoralePhase()
    {
        int newPhase = 0;
        if (moraleValue >= 1f) 
            newPhase = 4;
        else if (moraleValue >= 0.75f) 
            newPhase = 3;
        else if (moraleValue >= 0.5f)
            newPhase = 2;
        else if (moraleValue >= 0.25f)
            newPhase = 1;

        if(newPhase != currentPhase)
        {
            currentPhase = newPhase;
            TriggerPhaseEvenet(newPhase);
        }

    }

    void TriggerPhaseEvenet(int phase)
    {
        switch (phase)
        {
            case 1:

                break;
            case 2:
                EnemyMove.healthMultiplier = healthMultipliers[phase];
                break;
            case 3:
                EnemyMove.speedMultiplier = speedMultipliers[phase];
                break;
            case 4:
                if (isBossSpawned)
                {
                    ActivateBossSkill();
                }
                else
                {
                    SpawnBoss();
                    ResetMoraleGauge();//������ �ʱ�ȭ
                    isBossSpawned = true;

                }
                
                break;

        }
    }

    public void SpawnBoss()
    {
        GameObject boss = GameManager.instance.pool.Get(17);
        boss.transform.position = new Vector3(23, 0, 0);
        boss.transform.rotation = Quaternion.identity;

        GameManager.instance.bossNavigator.boss = boss.transform;
        GameManager.instance.bossNavigator.ShowNavigator();
        GameManager.instance.bossSpawned = true;
    }

    void ResetMoraleGauge()
    {
        moraleValue = 0f;
        moraleBar.fillAmount = moraleValue;
    }

    void ActivateBossSkill()
    {
        //������ ������ ��ų ���

        //������ ����
        moraleValue = 1f;
        moraleBar.fillAmount = moraleValue;
        StopCoroutine(DecayMorale() );
    }

}
