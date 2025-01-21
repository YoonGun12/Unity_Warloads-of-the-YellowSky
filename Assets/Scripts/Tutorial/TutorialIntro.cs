using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class TutorialIntro : MonoBehaviour
{
    public Image blackPanel;
    public TextMeshProUGUI introText;
    public TutorialManager tutorialManager;
    private string[] introLines =
    {
        "�ѳ����� ��Ʋ�� ��鸮��,\nȲ�� ��� �Ʒ� ���� ������ ����� �����ϴ�...",
        "������ ȥ���� ������,\n�鼺���� ���� �ӿ��� ���� �Ҿ�����...",
        "�׷����� �� �ڰ� ��θ� ������ ������,,,",        
    };

    private void Start()
    {
        introText.text = "";
        introText.color = new Color(introText.color.r, introText.color.g, introText.color.b, 0);
        blackPanel.color = new Color(0, 0, 0, 1);        
        StartCoroutine(ShowIntro());
    }

    IEnumerator ShowIntro()
    {
        foreach(string line in introLines)
        {
            introText.text = line;
            introText.DOFade(1, 2f);
            yield return new WaitForSeconds(3f);

            introText.DOFade(0, 2f);
            yield return new WaitForSeconds(2f);
        }

        blackPanel.DOFade(0, 2f).OnComplete(() =>
        {
            blackPanel.gameObject.SetActive(false);
            StartTutorial();
        });

        void StartTutorial()
        {
            tutorialManager.InitializeFadeIn();
        }
    }
}
