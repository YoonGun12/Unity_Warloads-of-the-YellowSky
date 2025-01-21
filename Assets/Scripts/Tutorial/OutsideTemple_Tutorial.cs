using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class OutsideTemple_Tutorial : MonoBehaviour
{
    public GameObject tree1; // 좌측으로 이동할 나무
    public GameObject tree2; // 우측으로 이동할 나무
    public Image fadePanel; // 페이드 효과를 위한 이미지

    public float treeSpreadDuration; // 나무 흩어지는 애니메이션 시간
    public float treeSpreadDistance; // 나무가 흩어지는 거리    

    private Vector3 initialTree1Pos; // Tree1의 초기 위치
    private Vector3 initialTree2Pos; // Tree2의 초기 위치

    public TextMeshProUGUI skip;

    private void Start()
    {
        // 나무의 초기 위치 저장
        initialTree1Pos = tree1.transform.position;
        initialTree2Pos = tree2.transform.position;

        BlinkSkip();

        // 페이드인 효과 후 나무 움직임 시작
        FadeIn(() =>
        {
            StartCoroutine(SpreadTrees());
        });
    }

    IEnumerator SpreadTrees()
    {
        // Tree1 왼쪽 이동
        tree1.transform.DOMoveX(initialTree1Pos.x - treeSpreadDistance, treeSpreadDuration)
            .SetEase(Ease.OutSine);

        yield return new WaitForSeconds(0.75f);

        // Tree2 오른쪽 이동
        tree2.transform.DOMoveX(initialTree2Pos.x + treeSpreadDistance, treeSpreadDuration)
            .SetEase(Ease.OutSine);

        yield return new WaitForSeconds(treeSpreadDuration);

        // 페이드아웃 효과 후 씬 전환
        FadeOut(ChangeScene);
    }

    void ChangeScene()
    {
        SceneManager.LoadScene("Tutorial");
        DOTween.KillAll();
    }

    void BlinkSkip()
    {
        skip.DOFade(0f, 1.5f).SetLoops(-1, LoopType.Yoyo);
    }

    public void SkipButton()
    {
        LoadingSceneController.LoadScene("TutorialBattle");
        DOTween.KillAll();
    }

    void FadeIn(TweenCallback onComplete)
    {
        fadePanel.color = new Color(0, 0, 0, 1); // 완전히 검게 설정
        fadePanel.gameObject.SetActive(true);
        fadePanel.DOFade(0f, 1.5f).OnComplete(() =>
        {
            fadePanel.gameObject.SetActive(false);
            onComplete?.Invoke();
        });
    }

    void FadeOut(TweenCallback onComplete)
    {
        fadePanel.color = new Color(0, 0, 0, 0); // 완전히 투명하게 설정
        fadePanel.gameObject.SetActive(true);
        fadePanel.DOFade(1f, 1.5f).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }
}
