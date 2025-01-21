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

    public void GameStartBtn() //��ŸƮ ��ư
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
        
    public void TogglePanel(GameObject panel) // �г� ����/�ݱ�
    {
        if (panel != null)
        {
            bool isActive = panel.activeSelf;
            panel.SetActive(!isActive);
            backgroundBtn.SetActive(!isActive); // ��� ��ư�� �Բ� Ȱ��ȭ/��Ȱ��ȭ
        }
    }

    public void OnBackgroundClick() // ��� Ŭ�� �� ��� �г� �ݱ�
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

    // �� ��ư�� ���� �ܼ�ȭ�� �޼���
    public void AchievementBtn() => TogglePanel(achievement);
    public void OptionBtn() => TogglePanel(optionPanel);
    public void ProductionStaffBtn() => TogglePanel(staff);


}
