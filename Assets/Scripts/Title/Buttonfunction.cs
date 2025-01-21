using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class Buttonfunction : MonoBehaviour
{
    public GameObject optionPanel;    
    public GameObject achievement;
    public GameObject staff;
    public GameObject backgroundBtn;
    public GameObject Arrow;
    public GameObject Arrow1;        

    public Text startText;

    private void Start()
    {
        Arrow.transform.DORotate(new Vector3(360f, 0f, 0f), 1f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1);
        Arrow1.transform.DORotate(new Vector3(360f, 180f, 0f), 1f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1);
    }


    public void StartButtonAnimation()
    {
        startText.DOFade(0, 0.1f).SetLoops(10, LoopType.Yoyo)
            .OnComplete(() => { GameStartBtn(); });
    }

    public void GameStartBtn() //스타트 버튼
    {
        DOTween.KillAll();
        if(PlayerPrefs.GetInt("HasPlayerdTutorial", 0) == 0)
        {
            LoadingSceneController.LoadScene("Begining_Tutorial");
        }
        else
        {
            LoadingSceneController.LoadScene("Town");
        }
        
    }
        
    public void TogglePanel(GameObject panel) // 패널 열기/닫기
    {
        if (panel != null)
        {
            bool isActive = panel.activeSelf;
            panel.SetActive(!isActive);
            backgroundBtn.SetActive(!isActive); // 배경 버튼도 함께 활성화/비활성화
        }
    }

    public void OnBackgroundClick() // 배경 클릭 시 모든 패널 닫기
    {
        if (optionPanel.activeSelf)
        {
            TogglePanel(optionPanel);
        }

        if (achievement.activeSelf)
        {
            TogglePanel(achievement);
        }

        if (staff.activeSelf)
        {
            TogglePanel(staff);
        }
    }

    // 각 버튼에 대한 단순화된 메서드
    public void AchievementBtn() => TogglePanel(achievement);
    public void OptionBtn() => TogglePanel(optionPanel);
    public void ProductionStaffBtn() => TogglePanel(staff);


}
