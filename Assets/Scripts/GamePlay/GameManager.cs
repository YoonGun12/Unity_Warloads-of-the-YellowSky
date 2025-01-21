using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{   
    [Header("# Game Control")]
    public float gameTime; //���� �ð�
    public float maxGameTime; //�ִ� ���� �ð�
    public bool isLive; //���� ���� ����

    [Header("# Game Object")]
    public PlayerMove player; //�÷��̾� ��ü
    public PoolManager pool; //Ǯ �Ŵ���    
    public static GameManager instance; //�̱��� �ν��Ͻ�
    public LevelUp uiLevelUp; //������ UI
    public Chest chest;    
    public BossNavigator bossNavigator;
    
    [Header("# Boss UI")]
    public bool bossSpawned = false;
    public bool bossDefeated = false;
    

    [Header("# Player Info")]
    public int level; //�÷��̾� ����
    public int kill; // �� óġ ��
    public int exp; //����ġ
    public int[] nextExp; //���� �������� �ʿ��� ����ġ(�ν����� â�� �ִ°� ���� ����Ǵ���)
    public float health; //�÷��̾� ü��
    public float maxhealth; //�ִ� ü��
    public Result uiResult;
    public GameObject enemyCleaner;
    public MoraleSystem moraleSystem;

    [Header("#Tutorial Mode")]
    public bool isTutorialMode;




    private void Awake()
    {
        instance = this; //�̱��� �ν��Ͻ� ����
        Application.targetFrameRate = 60;

        if (isTutorialMode)
        {
            maxGameTime = 60f;
        }
        else
        {
            maxGameTime = 180f;
        }

        GameStart();
    }

    public void GameStart()
    {
        health = maxhealth; //���� ���۽� Ǯ�Ƿ� ü�� �ʱ�ȭ
        player.gameObject.SetActive(true);
        uiLevelUp = FindObjectOfType<LevelUp>();
        uiLevelUp.StartGameItem(0);
        bossNavigator = FindObjectOfType<BossNavigator>();
        Resume();
    }

    public void GameQuit()
    {
        Application.Quit(); //���ø����̼� ����
    }

    void Update()
    {
        if (!isLive) return;
        
        gameTime += Time.deltaTime; //���� �ð� ������Ʈ

        if(isTutorialMode && gameTime >= maxGameTime && !bossSpawned)
        {
            moraleSystem.SpawnBoss();
            moraleSystem.isBossSpawned = true;
            bossSpawned = true;
        }

               
        if (bossSpawned && bossDefeated)
        {
            GameVictory();                        
        }
    }

    

    public void GetExp()
    {        
        if (!isLive)
        {
            return;
        }
        exp++; //����ġ ����                

        if(exp == nextExp[Mathf.Min(level, nextExp.Length-1)])
        {
            level++; //������
            exp = 0; //����ġ �ʱ�ȭ
            uiLevelUp.Show(); //������ UI ǥ��
        }
    }

    public void Stop()
    {
        isLive = false; //���� ������� ��Ȱ��ȭ
        Time.timeScale = 0; //���� �Ͻ�����
    }

    public void Resume()
    {
        isLive = true; //���� ������� Ȱ��ȭ
        Time.timeScale = 1; //���� �簳
    }
    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(1.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();
    }
    public void Retry()
    {
        SceneManager.LoadScene(2);
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        if (isTutorialMode)
        {
            PlayerPrefs.SetInt("HasPlayedTutorial", 1);
            PlayerPrefs.Save();
            DOTween.KillAll();
            LoadingSceneController.LoadScene("Town");
        }
        else
        {
            uiResult.gameObject.SetActive(true);
            uiResult.Win();
            Stop();
        }
        
    }
        

    public void DefeatBoss()
    {
        bossDefeated = true;
    }
        
}
