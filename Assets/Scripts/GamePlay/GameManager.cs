using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{   
    [Header("# Game Control")]
    public float gameTime; //게임 시간
    public float maxGameTime; //최대 게임 시간
    public bool isLive; //게임 진행 상태

    [Header("# Game Object")]
    public PlayerMove player; //플레이어 객체
    public PoolManager pool; //풀 매니저    
    public static GameManager instance; //싱글톤 인스턴스
    public LevelUp uiLevelUp; //레벨업 UI
    public Chest chest;    
    public BossNavigator bossNavigator;
    
    [Header("# Boss UI")]
    public bool bossSpawned = false;
    public bool bossDefeated = false;
    

    [Header("# Player Info")]
    public int level; //플레이어 레벨
    public int kill; // 적 처치 수
    public int exp; //경험치
    public int[] nextExp; //다음 레벨까지 필요한 경험치(인스펙터 창에 있는게 먼저 적용되는중)
    public float health; //플레이어 체력
    public float maxhealth; //최대 체력
    public Result uiResult;
    public GameObject enemyCleaner;
    public MoraleSystem moraleSystem;

    [Header("#Tutorial Mode")]
    public bool isTutorialMode;




    private void Awake()
    {
        instance = this; //싱글톤 인스턴스 설정
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
        health = maxhealth; //게임 시작시 풀피로 체력 초기화
        player.gameObject.SetActive(true);
        uiLevelUp = FindObjectOfType<LevelUp>();
        uiLevelUp.StartGameItem(0);
        bossNavigator = FindObjectOfType<BossNavigator>();
        Resume();
    }

    public void GameQuit()
    {
        Application.Quit(); //애플리케이션 종료
    }

    void Update()
    {
        if (!isLive) return;
        
        gameTime += Time.deltaTime; //게임 시간 업데이트

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
        exp++; //경험치 증가                

        if(exp == nextExp[Mathf.Min(level, nextExp.Length-1)])
        {
            level++; //레벨업
            exp = 0; //경험치 초기화
            uiLevelUp.Show(); //레벨업 UI 표시
        }
    }

    public void Stop()
    {
        isLive = false; //게임 진행상태 비활성화
        Time.timeScale = 0; //게임 일시정지
    }

    public void Resume()
    {
        isLive = true; //게임 진행상태 활성화
        Time.timeScale = 1; //게임 재개
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
