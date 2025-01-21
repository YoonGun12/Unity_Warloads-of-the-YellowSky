using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause_Option : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject optionPanel;    
    public GameObject backgroundBtn;

    bool isOptionOpen = false;

    public void PauseButton()
    {
        OpenPausePanel();
        GameManager.instance.Stop();      
    }

    public void ResumeButton()
    {
        CloseAllUI();
        GameManager.instance.Resume();
    }

    public void OptionButton()
    {
        isOptionOpen = true;
        OpenUI(optionPanel);
    }

    public void CloseOptionButton()
    {
        isOptionOpen = false ;
        OpenUI(pausePanel);
    }

    public void OnBackgroundClick()
    {
        if(isOptionOpen)
        {
            isOptionOpen = false;
            OpenPausePanel();
        }
        else
        {
            ResumeButton();
        }
    }

    void OpenPausePanel()
    {
        OpenUI(pausePanel);
    }

    void OpenUI(GameObject targetUI)
    {
        CloseAllUI();
        targetUI.SetActive(true);
        backgroundBtn.SetActive(true);
    }

    void CloseAllUI()
    {
        if(pausePanel.activeSelf) pausePanel.SetActive(false);
        if(optionPanel.activeSelf) optionPanel.SetActive(false);
        if(backgroundBtn.activeSelf) backgroundBtn.SetActive(false);
    }

    public void TutorialQuit()
    {
        LoadingSceneController.LoadScene("Town");
    }
}
