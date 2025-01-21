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
        "한나라의 기틀이 흔들리고,\n황색 깃발 아래 악한 무리가 들끓기 시작하니...",
        "전국이 혼란에 빠지고,\n백성들은 절망 속에서 길을 잃었도다...",
        "그렇지만 이 자가 모두를 구원해 줄지니,,,",        
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
