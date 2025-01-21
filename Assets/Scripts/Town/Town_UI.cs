using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town_UI : MonoBehaviour
{
    //UI 구성요소
    public GameObject upgradeUI;
    public GameObject gameStartUI;
    public GameObject characterUI;
    public GameObject pausePanel;
    public GameObject optionPanel;
    public GameObject DrawingBookUI;
    public GameObject ShopUI;

    //뒤로가기 버튼
    public GameObject backgroundBtn;

    private GameObject[] allUIs;

    private void Awake()
    {
        allUIs = new GameObject[] { upgradeUI, gameStartUI, characterUI, pausePanel, optionPanel, DrawingBookUI, ShopUI };
    }

    public void OpenUI(GameObject targetUI)
    {
        CloseAllUI();
        targetUI.SetActive(true);
        backgroundBtn.SetActive(true);
    }

    public void CloseAllUI()
    {
        foreach (var ui in allUIs)
        {
            if(ui.activeSelf)
            {
                ui.SetActive(false);
            }
        }

        backgroundBtn.SetActive(false);
    }

    public void BackgroundBtn()
    {
        if (pausePanel.activeSelf)
        {
            ResumeBtn();
        }
        else
        {
            CloseAllUI();
        }
    }
    public void UpgradeBtn()
    {
        OpenUI(upgradeUI);
    }
    public void StartBtn()
    {
        OpenUI(gameStartUI);
    }
    public void CharacterBtn()
    {
        OpenUI(characterUI);
    }
    public void DrawingBookBtn()
    {
        OpenUI(DrawingBookUI);
    }
    public void ShopBtn()
    {
        OpenUI(ShopUI);
    }
    public void PauseBtn()
    {
        OpenUI(pausePanel);
    }
    public void ResumeBtn()
    {
        CloseAllUI();
    }
    public void OptionBtn()
    {
        OpenUI(optionPanel);
    }
    public void CloseOptionBtn()
    {
        OpenUI(pausePanel);
    }
    public void TutorialBtn()
    {
        DOTween.KillAll();
        LoadingSceneController.LoadScene("Begining_Tutorial");
    }
    public void GameQuitBtn()
    {
        Application.Quit();
    }
    public void LoadStage()
    {
        DOTween.KillAll();
        LoadingSceneController.LoadScene("GamePlay");
    }
}
